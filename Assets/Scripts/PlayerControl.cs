using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerControl : MonoBehaviour, ISoundMaker
{
    public static PlayerControl Instance { get; private set; }
    public enum PlayerState
    {
        NONE, //can go to any state
        BUSY, //can't go to any state but NONE
        JUMP, //only if you're not busy
        FALL,
        LAND,
        SNEAK, //only if you're not jumping (same for others)
        WALK,
        CROUCH,
        SIT
    }
    public PlayerState state;
    public bool startSneaky;
    public bool isSmall;

    [Header("Sounds")]
    public AudioManager.AudioTrack[] sounds;

    #region References
    [Header("Input Actions")]
    public PlayerInput input;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction interactAction;
    InputAction sneakAction;
    InputAction phoneAction;
    InputAction resetAction;
    InputAction freeAction;
    [Header("References")]
    public Animator animator;
    public GameObject interactArrow;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer altSpriteRenderer;
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private AudioSource sfxPlayer;

    private DialogueRunner dr;
    public float interactMinDistance;
    private List<Interactable> interactList = new List<Interactable>();
    private Interactable interactSelection; 
    private int interactIndex;

    #endregion

    #region Sprites
    [Header("Sprites")]
    public FaceManager.Face[] altSprites;
    #endregion

    #region Variables
    [Header("Movement")]
    private bool canMove;
    private Vector2 move;
    public float walkSpeed;
    public float sneakSpeed;

    [Header("Fall and Jump")]
    public LayerMask jumpLayerMask;
    public float jumpSpeed;
    public float jumpTime;
    public float landTime;
    public float jumpBufferTime;
    float timeSinceJumpInput;
    public bool isGrounded;
    public float fallSpeed;
    public float fallFastMultiplier;
    [SerializeField]
    private float fallSpeedActual;
    private bool isFastfall;
    private bool isFalling;

    [Header("Stealth")]
    public LayerMask stealthLayerMask;
    public Vector3 soundOffset;
    public float sneakRadius;
    public float walkRadius;
    public bool isDetectable;

    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            moveAction = input.actions["Move"];
            jumpAction = input.actions["Jump"];
            interactAction = input.actions["Interact"];
            sneakAction = input.actions["Sneak"];
            phoneAction = input.actions["TogglePhone"];
            resetAction = input.actions["Reset"];
            freeAction = input.actions["Free"];
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        dr = FindObjectOfType<DialogueRunner>();
        interactIndex = -1;

        canMove = true;
        fallSpeedActual = fallSpeed;
        isFastfall = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && state != PlayerState.BUSY && !GameManager.Instance.isBusy && !EssayGrader.isUp)
        {
            CheckFall();
            CheckGround();
            Move();

            if (jumpAction.phase == InputActionPhase.Started)
            {
                Jump();
            }
            if (isGrounded && interactAction.phase == InputActionPhase.Started)
            {
                if (interactList.Count > 0)
                {
                    //Pause();
                    SelectInteractable(moveAction.ReadValue<Vector2>().x);

                }
            }
            else if (interactAction.phase == InputActionPhase.Canceled)
            {
                if (interactSelection != null)
                {
                    interactSelection.Interact();
                    interactArrow.SetActive(false);
                    sfxPlayer.PlayOneShot(sounds[3].audioClip, 0.2f);
                }
            }
            if (phoneAction.phase == InputActionPhase.Started)
            {
                Debug.Log(PhoneManager.Instance.phoneState);
                if (PhoneManager.Instance.isFocused)
                {
                    PhoneManager.Instance.Unfocus();
                }
                else
                {
                    PhoneManager.Instance.Focus();
                }
            }
        }
        else
        {
            Pause();
        }
        if (!isGrounded && jumpAction.phase == InputActionPhase.Canceled)
        {
            Fastfall();
        }

        if (resetAction.phase == InputActionPhase.Started)
        {
            GameManager.Instance.isBusy = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (!GameManager.Instance.debugMode)
            return;
        if (freeAction.phase == InputActionPhase.Started)
        {
            state = PlayerState.NONE;
            GameManager.Instance.inConvo = false;
            GameManager.Instance.isBusy = false;
        }
    }
    void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(move.x, 0, 0) * Time.fixedDeltaTime;
    }
    public void PlaySound(int index)
    {
        AudioManager.AudioTrack a = sounds[index];
        if (a != null && a.audioClip != null)
        {
            sfxPlayer.PlayOneShot(a.audioClip);
        }
    }
    public void SetPlayerState(PlayerState setting)
    {
        state = setting;
        if (state == PlayerState.BUSY)
        {
            Pause();
        }
        //Debug.Log(setting);
    }

    #region Movement Functions

    void Move()
    {
        move = moveAction.ReadValue<Vector2>();
        if (move.x > 0.1)
        {
            spriteRenderer.flipX = false;
            SetMoveSpeed();
        }
        else if (move.x < -0.1)
        {
            spriteRenderer.flipX = true;
            SetMoveSpeed();
        }
        else if (isGrounded)
        {
            if (sneakAction.triggered)
            {
                animator.SetInteger("State", (int)PlayerState.CROUCH);
                AdjustCollider(true);
            }
            else
            {
                animator.SetInteger("State", (int)PlayerState.NONE);
                AdjustCollider(false);
            }
        }
    }
    void AdjustCollider(bool isCrouch)
    {
        if (!isSmall)
        {
            if (isCrouch)
            {
                col.offset = new Vector2(0, .75f);
                col.size = new Vector2(.75f, 1.5f);
            }
            else
            {
                col.offset = new Vector2(0, 1.875f);
                col.size = new Vector2(.75f, 3.75f);
            }
        }
    }
    void SetMoveSpeed()
    {
        var test = Gamepad.current;
        //if (test.)
        if (!isGrounded)
        {
            move *= walkSpeed;
        }
        else
        {
            if (startSneaky || sneakAction.triggered)
            {
                animator.SetInteger("State", (int)PlayerState.SNEAK);
                move *= sneakSpeed;
            }
            else
            {
                animator.SetInteger("State", (int)PlayerState.WALK);
                move *= walkSpeed;
            }
        }
        AdjustCollider(false);
    }
    void Jump()
    {
        if (LevelLoader.Instance != null && LevelLoader.Instance.currentScene != "ChinatownOutside" && LevelLoader.Instance.currentScene != "Outside test")
        {
            return;
        }
        timeSinceJumpInput = Time.time;
        if (isGrounded)
        {
            //Debug.Log("Trying to jump!");
            StopAllCoroutines();

            move = Vector2.zero;
            state = PlayerState.JUMP;
            animator.SetInteger("State", (int)state);
            //StartCoroutine(IJump());
            //TESTING
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            
            fallSpeedActual = fallSpeed;
        }
    }
    void Fall()
    {
        isFalling = true;
        rb.gravityScale = fallSpeedActual;
        state = PlayerState.FALL;
        animator.SetInteger("State", (int)state);
    }
    void Fastfall()
    {
        //TESTING: Removed ground check
        if (!isFastfall)
        {
            //Debug.Log("Fastfalling");
            isFastfall = true;
            //TESTING
            fallSpeedActual = fallSpeed * fallFastMultiplier;
            rb.gravityScale = fallSpeedActual;
        }
    }
    void Land()
    {
        isFalling = false;
        //Debug.Log("Grounded");
        StopAllCoroutines();

        isFastfall = false;
        fallSpeedActual = fallSpeed;
        rb.gravityScale = fallSpeedActual;

        if ((Time.time - timeSinceJumpInput) < jumpBufferTime)
        {
            Debug.Log("buffered jump!");
            //Jump();
        }
        else
        {
            state = PlayerState.LAND;
            animator.SetInteger("State", (int)state);
            //StartCoroutine(ILand());
            //TESTING
            sfxPlayer.PlayOneShot(sounds[2].audioClip);
            state = PlayerState.NONE;
            animator.SetInteger("State", (int)state);
        }
    }

    void CheckFall()
    {
        if (!isGrounded && rb.velocity.y < -.05)
        {
            Fall();
        }
    }
    void CheckGround()
    {
        Bounds colBounds = col.bounds;
        Vector2 colBox = new Vector2(colBounds.size.x - .25f, colBounds.size.y);
        Vector2 colCenter = new Vector2(colBounds.center.x, colBounds.center.y - .05f);
        Collider2D raycastHit = Physics2D.OverlapBox(colCenter, colBox, 0, jumpLayerMask);
        //Debug.Log(raycastHit != null);
        bool hitGround = raycastHit != null;
        if (!isGrounded && hitGround)
        {
            isGrounded = hitGround;
            Land();
        }
        isGrounded = hitGround;
    }

    IEnumerator IJump()
    {
        yield return StopMovement(jumpTime);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        fallSpeedActual = fallSpeed;
    }
    IEnumerator ILand()
    {
        yield return StopMovement(landTime);
        state = PlayerState.NONE;
        animator.SetInteger("State", (int)state);
    }
    public void Pause()
    {
        move = Vector2.zero;
        animator.SetInteger("State", (int)PlayerState.NONE);
    }
    public void AllowMovement(bool setting)
    {
        canMove = setting;
    }
    public IEnumerator StopMovement(float waitTime)
    {
        move = Vector2.zero;
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }
    public void Spawn(Vector3 location, bool setting)
    {
        transform.position = location;
        spriteRenderer.flipX = setting;
    }
    #endregion

    [YarnCommand("showplayer")]
    public void ShowPlayer(string param)
    {
        
        bool.TryParse(param, out bool setting);
        spriteRenderer.enabled = setting;
        canMove = setting;
        Debug.Log("showing player " + setting);
        interactArrow.GetComponent<SpriteRenderer>().enabled = setting;

    }
    [YarnCommand("usealt")]
    public void UseAlt(string spriteName)
    {
        foreach (FaceManager.Face f in altSprites)
        {
            if (f.expression == spriteName) {
                altSpriteRenderer.sprite = f.sprite;
                altSpriteRenderer.enabled = true;
                spriteRenderer.enabled = false;
            }
        }
    }
    [YarnCommand("usebase")]
    public void UseBase()
    {
        foreach (FaceManager.Face f in altSprites)
        {
            altSpriteRenderer.sprite = f.sprite;
            altSpriteRenderer.enabled = false;
            spriteRenderer.enabled = true;
        }
    }

    void CheckInteractables()
    {
        Debug.Log("checking interactables");
        interactList.Sort();
        foreach (Interactable i in interactList)
        {
            Debug.Log($"{i.interactableName} {Mathf.Abs(transform.position.x - i.transform.position.x)}");
        }
        interactSelection = interactList[0];
    }
    public void AddInteractable(Interactable i)
    {
        if (interactList == null)
            Debug.Log("failed");
        interactList.Add(i);
        Debug.Log($"added {i.interactableName}");
    }
    public void SelectInteractable(float f)
    {
        //Debug.Log(interactList.Count);
        
        if (f > 0)
        {
            interactIndex++;
            if (interactIndex >= interactList.Count)
            {
                interactIndex = 0;
            }
        }
        else if (f < 0)
        {
            interactIndex--;
            if (interactIndex < 0)
            {
                interactIndex = interactList.Count - 1;
            }
        }
        PlaceInteractArrow(interactList[interactIndex]);
    }
    public void PlaceInteractArrow(Interactable interact)
    {
        if (interact.showArrow)
        {
            //Debug.Log("showing");
            interactArrow.SetActive(true);
            switch (interact.arrowDirection)
            {
                case (Interactable.ArrowDirection.DOWN):
                    interactArrow.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case (Interactable.ArrowDirection.LEFT):
                    interactArrow.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case (Interactable.ArrowDirection.RIGHT):
                    interactArrow.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case (Interactable.ArrowDirection.UP):
                    interactArrow.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
            }
            interactArrow.transform.localScale = interact.arrowScale;
            interactArrow.transform.position = interact.transform.position + interact.arrowOffset;
        }
    }

    public void MakeSound()
    {
        if (isDetectable)
        {
            Collider2D[] colliders;
            if (state == PlayerControl.PlayerState.SNEAK)
            {
                colliders = Physics2D.OverlapCircleAll(transform.position + soundOffset, sneakRadius, stealthLayerMask);
            }
            else
            {
                colliders = Physics2D.OverlapCircleAll(transform.position + soundOffset, walkRadius, stealthLayerMask);
            }
            if (colliders.Length > 0)
            {
                foreach (Collider2D c in colliders)
                {
                    c.gameObject.GetComponent<ISoundListener>().ListenSound(transform.position);
                }
            }
        }
        else
        {
            //Debug.Log("Made sound, but not detectable right now");
        }
    }
    public void Mute()
    {

    }
    public void Unmute()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Interactable interact = collision.gameObject.GetComponent<Interactable>();
            //Debug.Log("Contact " + interact.interactableName);
            if (interact != null && interact.isActiveAndEnabled)
            {
                interactList.Add(interact);

                interactIndex = interactList.Count - 1;
                interactSelection = interact;

                PlaceInteractArrow(interact);

                if (interactSelection.startOnCollision)
                {
                    Debug.Log("test");
                    interactSelection.Interact();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //One possible solution: just do a distance check instead of removing anything
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Interactable interact = collision.gameObject.GetComponent<Interactable>();
            //Debug.Log("Uncontact " + interact.interactableName);
            interactList.Remove(interact);

            interactIndex = interactList.Count - 1;

            if (interactList.Count < 1)
            {
                interactArrow.SetActive(false);
                interactSelection = null;
            }
            else
            {
                PlaceInteractArrow(interactList[interactList.Count - 1]);
                interactSelection = interactList[interactList.Count - 1];
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + soundOffset, sneakRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + soundOffset, walkRadius);
    }
}
