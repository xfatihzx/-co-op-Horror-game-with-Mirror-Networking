using UnityEngine;

public class MonoBehaviourEventable : MonoBehaviour
{
    public delegate void MiddleStartNoArgs();
    public delegate void MiddleStartInt(int val1);
    public delegate void MiddleStartBool(bool val1);
    public delegate void MiddleStartFloat0x0Int(float[] val1, int val2);
    public delegate void MiddleStartCollision(Collision val1);
    public delegate void MiddleStartCollision2D(Collision2D val1);
    public delegate void MiddleStartControllerColliderHit(ControllerColliderHit val1);
    public delegate void MiddleStartFloat(float val1);
    public delegate void MiddleStartJoint2D(Joint2D val1);
    public delegate void MiddleStartGameObject(GameObject val1);
    public delegate void MiddleStartRenderTextureRenderTexture(RenderTexture val1, RenderTexture val2);
    public delegate void MiddleStartCollider(Collider val1);
    public delegate void MiddleStartCollider2D(Collider2D val1);

    // Awake is called when the script instance is being loaded
    public event MiddleStartNoArgs AsAwake;
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    public event MiddleStartNoArgs AsFixedUpdate;
    // LateUpdate is called every frame, if the Behaviour is enabled
    public event MiddleStartNoArgs AsLateUpdate;
    // Callback for setting up animation IK (inverse kinematics)
    public event MiddleStartInt AsOnAnimatorIK;
    // This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK
    public event MiddleStartNoArgs AsOnAnimatorMove;
    // Sent to all game objects when the player gets or loses focus
    public event MiddleStartBool AsOnApplicationFocus;
    // Sent to all game objects when the player pauses
    public event MiddleStartBool AsOnApplicationPause;
    // Sent to all game objects before the application is quit
    public event MiddleStartNoArgs AsOnApplicationQuit;
    // If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain
    public event MiddleStartFloat0x0Int AsOnAudioFilterRead;
    // OnBecameInvisible is called when the renderer is no longer visible by any camera
    public event MiddleStartNoArgs AsOnBecameInvisible;
    // OnBecameVisible is called when the renderer became visible by any camera
    public event MiddleStartNoArgs AsOnBecameVisible;
    // Callback sent to the graphic before a Transform parent change occurs
    public event MiddleStartNoArgs AsOnBeforeTransformParentChanged;
    // Callback that is sent if the canvas group is changed
    public event MiddleStartNoArgs AsOnCanvasGroupChanged;
    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public event MiddleStartCollision AsOnCollisionEnter;
    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    public event MiddleStartCollision2D AsOnCollisionEnter2D;
    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider
    public event MiddleStartCollision AsOnCollisionExit;
    // OnCollisionExit2D is called when this collider2D/rigidbody2D has stopped touching another rigidbody2D/collider2D (2D physics only)
    public event MiddleStartCollision2D AsOnCollisionExit2D;
    // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider
    public event MiddleStartCollision AsOnCollisionStay;
    // OnCollisionStay2D is called once per frame for every collider2D/rigidbody2D that is touching rigidbody2D/collider2D (2D physics only)
    public event MiddleStartCollision2D AsOnCollisionStay2D;
    // Called on the client when you have successfully connected to a server
    public event MiddleStartNoArgs AsOnConnectedToServer;
    // OnControllerColliderHit is called when the controller hits a collider while performing a Move
    public event MiddleStartControllerColliderHit AsOnControllerColliderHit;
    // This function is called when the MonoBehaviour will be destroyed
    public event MiddleStartNoArgs AsOnDestroy;
    // This function is called when the behaviour becomes disabled or inactive
    public event MiddleStartNoArgs AsOnDisable;
    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    public event MiddleStartNoArgs AsOnDrawGizmos;
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    public event MiddleStartNoArgs AsOnDrawGizmosSelected;
    // This function is called when the object becomes enabled and active
    public event MiddleStartNoArgs AsOnEnable;
    // OnGUI is called for rendering and handling GUI events
    public event MiddleStartNoArgs AsOnGUI;
    // Called when a joint attached to the same game object broke
    public event MiddleStartFloat AsOnJointBreak;
    // Called when a Joint2D attached to the same game object broke (2D physics only)
    public event MiddleStartJoint2D AsOnJointBreak2D;
    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public event MiddleStartNoArgs AsOnMouseDown;
    // OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
    public event MiddleStartNoArgs AsOnMouseDrag;
    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public event MiddleStartNoArgs AsOnMouseEnter;
    // OnMouseExit is called when the mouse is no longer over the GUIElement or Collider
    public event MiddleStartNoArgs AsOnMouseExit;
    // OnMouseOver is called every frame while the mouse is over the GUIElement or Collider
    public event MiddleStartNoArgs AsOnMouseOver;
    // OnMouseUp is called when the user has released the mouse button
    public event MiddleStartNoArgs AsOnMouseUp;
    // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed
    public event MiddleStartNoArgs AsOnMouseUpAsButton;
    // OnParticleCollision is called when a particle hits a collider
    public event MiddleStartGameObject AsOnParticleCollision;
    // Called when all particles in the system have died, and no new particles will be born
    public event MiddleStartNoArgs AsOnParticleSystemStopped;
    // Called when any particles in a particle system meet the conditions in the trigger module
    public event MiddleStartNoArgs AsOnParticleTrigger;
    // Called when a Particle System's built-in update job has been scheduled
    public event MiddleStartNoArgs AsOnParticleUpdateJobScheduled;
    // OnPostRender is called after a camera finished rendering the scene
    public event MiddleStartNoArgs AsOnPostRender;
    // OnPreCull is called before a camera culls the scene
    public event MiddleStartNoArgs AsOnPreCull;
    // OnPreRender is called before a camera starts rendering the scene
    public event MiddleStartNoArgs AsOnPreRender;
    // Callback that is sent if an associated RectTransform has it's dimensions changed
    public event MiddleStartNoArgs AsOnRectTransformDimensionsChange;
    // Callback that is sent if an associated RectTransform is removed
    public event MiddleStartNoArgs AsOnRectTransformRemoved;
    // OnRenderImage is called after all rendering is complete to render image
    public event MiddleStartRenderTextureRenderTexture AsOnRenderImage;
    // OnRenderObject is called after camera has rendered the scene
    public event MiddleStartNoArgs AsOnRenderObject;
    // Called on the server whenever a Network.InitializeServer was invoked and has completed
    public event MiddleStartNoArgs AsOnServerInitialized;
    // Callback sent to the graphic afer a Transform children change occurs
    public event MiddleStartNoArgs AsOnTransformChildrenChanged;
    // Callback sent to the graphic afer a Transform parent change occurs
    public event MiddleStartNoArgs AsOnTransformParentChanged;
    public event MiddleStartCollider AsOnTriggerEnter;
    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public event MiddleStartCollider2D AsOnTriggerEnter2D;
    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    public event MiddleStartCollider AsOnTriggerExit;
    // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
    public event MiddleStartCollider2D AsOnTriggerExit2D;
    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public event MiddleStartCollider AsOnTriggerStay;
    // OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only)
    public event MiddleStartCollider2D AsOnTriggerStay2D;
    // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
    public event MiddleStartNoArgs AsOnValidate;
    // OnWillRenderObject is called once for each camera if the object is visible
    public event MiddleStartNoArgs AsOnWillRenderObject;
    // Reset to default values
    public event MiddleStartNoArgs AsReset;
    // Start is called just before any of the Update methods is called the first time
    public event MiddleStartNoArgs AsStart;
    // Update is called every frame, if the MonoBehaviour is enabled
    public event MiddleStartNoArgs AsUpdate;

    // Awake is called when the script instance is being loaded
    public virtual void Awake()
    {
        AsAwake?.Invoke();
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    public virtual void FixedUpdate()
    {
        AsFixedUpdate?.Invoke();
    }
    // LateUpdate is called every frame, if the Behaviour is enabled
    public virtual void LateUpdate()
    {
        AsLateUpdate?.Invoke();
    }
    // Callback for setting up animation IK (inverse kinematics)
    public virtual void OnAnimatorIK(int layerIndex)
    {
        AsOnAnimatorIK?.Invoke(layerIndex);
    }
    // This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK
    public virtual void OnAnimatorMove()
    {
        AsOnAnimatorMove?.Invoke();
    }
    // Sent to all game objects when the player gets or loses focus
    public virtual void OnApplicationFocus(bool focus)
    {
        AsOnApplicationFocus?.Invoke(focus);
    }
    // Sent to all game objects when the player pauses
    public virtual void OnApplicationPause(bool pause)
    {
        AsOnApplicationPause?.Invoke(pause);
    }
    // Sent to all game objects before the application is quit
    public virtual void OnApplicationQuit()
    {
        AsOnApplicationQuit?.Invoke();
    }
    // If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain
    public virtual void OnAudioFilterRead(float[] data, int channels)
    {
        AsOnAudioFilterRead?.Invoke(data, channels);
    }
    // OnBecameInvisible is called when the renderer is no longer visible by any camera
    public virtual void OnBecameInvisible()
    {
        AsOnBecameInvisible?.Invoke();
    }
    // OnBecameVisible is called when the renderer became visible by any camera
    public virtual void OnBecameVisible()
    {
        AsOnBecameVisible?.Invoke();
    }
    // Callback sent to the graphic before a Transform parent change occurs
    public virtual void OnBeforeTransformParentChanged()
    {
        AsOnBeforeTransformParentChanged?.Invoke();
    }
    // Callback that is sent if the canvas group is changed
    public virtual void OnCanvasGroupChanged()
    {
        AsOnCanvasGroupChanged?.Invoke();
    }
    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public virtual void OnCollisionEnter(Collision collision)
    {
        AsOnCollisionEnter?.Invoke(collision);
    }
    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        AsOnCollisionEnter2D?.Invoke(collision);
    }
    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider
    public virtual void OnCollisionExit(Collision collision)
    {
        AsOnCollisionExit?.Invoke(collision);
    }
    // OnCollisionExit2D is called when this collider2D/rigidbody2D has stopped touching another rigidbody2D/collider2D (2D physics only)
    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        AsOnCollisionExit2D?.Invoke(collision);
    }
    // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider
    public virtual void OnCollisionStay(Collision collision)
    {
        AsOnCollisionStay?.Invoke(collision);
    }
    // OnCollisionStay2D is called once per frame for every collider2D/rigidbody2D that is touching rigidbody2D/collider2D (2D physics only)
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        AsOnCollisionStay2D?.Invoke(collision);
    }
    // Called on the client when you have successfully connected to a server
    public virtual void OnConnectedToServer()
    {
        AsOnConnectedToServer?.Invoke();
    }
    // OnControllerColliderHit is called when the controller hits a collider while performing a Move
    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
        AsOnControllerColliderHit?.Invoke(hit);
    }
    // This function is called when the MonoBehaviour will be destroyed
    public virtual void OnDestroy()
    {
        AsOnDestroy?.Invoke();
    }
    // This function is called when the behaviour becomes disabled or inactive
    public virtual void OnDisable()
    {
        AsOnDisable?.Invoke();
    }
    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    public virtual void OnDrawGizmos()
    {
        AsOnDrawGizmos?.Invoke();
    }
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    public virtual void OnDrawGizmosSelected()
    {
        AsOnDrawGizmosSelected?.Invoke();
    }
    // This function is called when the object becomes enabled and active
    public virtual void OnEnable()
    {
        AsOnEnable?.Invoke();
    }
    // OnGUI is called for rendering and handling GUI events
    public virtual void OnGUI()
    {
        AsOnGUI?.Invoke();
    }
    // Called when a joint attached to the same game object broke
    public virtual void OnJointBreak(float breakForce)
    {
        AsOnJointBreak?.Invoke(breakForce);
    }
    // Called when a Joint2D attached to the same game object broke (2D physics only)
    public virtual void OnJointBreak2D(Joint2D joint)
    {
        AsOnJointBreak2D?.Invoke(joint);
    }
    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public virtual void OnMouseDown()
    {
        AsOnMouseDown?.Invoke();
    }
    // OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
    public virtual void OnMouseDrag()
    {
        AsOnMouseDrag?.Invoke();
    }
    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public virtual void OnMouseEnter()
    {
        AsOnMouseEnter?.Invoke();
    }
    // OnMouseExit is called when the mouse is no longer over the GUIElement or Collider
    public virtual void OnMouseExit()
    {
        AsOnMouseExit?.Invoke();
    }
    // OnMouseOver is called every frame while the mouse is over the GUIElement or Collider
    public virtual void OnMouseOver()
    {
        AsOnMouseOver?.Invoke();
    }
    // OnMouseUp is called when the user has released the mouse button
    public virtual void OnMouseUp()
    {
        AsOnMouseUp?.Invoke();
    }
    // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed
    public virtual void OnMouseUpAsButton()
    {
        AsOnMouseUpAsButton?.Invoke();
    }
    // OnParticleCollision is called when a particle hits a collider
    public virtual void OnParticleCollision(GameObject other)
    {
        AsOnParticleCollision?.Invoke(other);
    }
    // Called when all particles in the system have died, and no new particles will be born
    public virtual void OnParticleSystemStopped()
    {
        AsOnParticleSystemStopped?.Invoke();
    }
    // Called when any particles in a particle system meet the conditions in the trigger module
    public virtual void OnParticleTrigger()
    {
        AsOnParticleTrigger?.Invoke();
    }
    // Called when a Particle System's built-in update job has been scheduled
    public virtual void OnParticleUpdateJobScheduled()
    {
        AsOnParticleUpdateJobScheduled?.Invoke();
    }
    // OnPostRender is called after a camera finished rendering the scene
    public virtual void OnPostRender()
    {
        AsOnPostRender?.Invoke();
    }
    // OnPreCull is called before a camera culls the scene
    public virtual void OnPreCull()
    {
        AsOnPreCull?.Invoke();
    }
    // OnPreRender is called before a camera starts rendering the scene
    public virtual void OnPreRender()
    {
        AsOnPreRender?.Invoke();
    }
    // Callback that is sent if an associated RectTransform has it's dimensions changed
    public virtual void OnRectTransformDimensionsChange()
    {
        AsOnRectTransformDimensionsChange?.Invoke();
    }
    // Callback that is sent if an associated RectTransform is removed
    public virtual void OnRectTransformRemoved()
    {
        AsOnRectTransformRemoved?.Invoke();
    }
    // OnRenderImage is called after all rendering is complete to render image
    public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        AsOnRenderImage?.Invoke(source, destination);
    }
    // OnRenderObject is called after camera has rendered the scene
    public virtual void OnRenderObject()
    {
        AsOnRenderObject?.Invoke();
    }
    // Called on the server whenever a Network.InitializeServer was invoked and has completed
    public virtual void OnServerInitialized()
    {
        AsOnServerInitialized?.Invoke();
    }
    // Callback sent to the graphic afer a Transform children change occurs
    public virtual void OnTransformChildrenChanged()
    {
        AsOnTransformChildrenChanged?.Invoke();
    }
    // Callback sent to the graphic afer a Transform parent change occurs
    public virtual void OnTransformParentChanged()
    {
        AsOnTransformParentChanged?.Invoke();
    }
    // OnTriggerEnter is called when the Collider other enters the trigger
    public virtual void OnTriggerEnter(Collider other)
    {
        AsOnTriggerEnter?.Invoke(other);
    }
    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        AsOnTriggerEnter2D?.Invoke(collision);
    }
    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    public virtual void OnTriggerExit(Collider other)
    {
        AsOnTriggerExit?.Invoke(other);
    }
    // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        AsOnTriggerExit2D?.Invoke(collision);
    }
    // OnTriggerStay is called once per frame for every Collider other that is touching the trigger
    public virtual void OnTriggerStay(Collider other)
    {
        AsOnTriggerStay?.Invoke(other);
    }
    // OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only)
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        AsOnTriggerStay2D?.Invoke(collision);
    }
    // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
    public virtual void OnValidate()
    {
        AsOnValidate?.Invoke();
    }
    // OnWillRenderObject is called once for each camera if the object is visible
    public virtual void OnWillRenderObject()
    {
        AsOnWillRenderObject?.Invoke();
    }
    // Reset to default values
    public virtual void Reset()
    {
        AsReset?.Invoke();
    }
    // Start is called just before any of the Update methods is called the first time
    public virtual void Start()
    {
        AsStart?.Invoke();
    }
    // Update is called every frame, if the MonoBehaviour is enabled
    public virtual void Update()
    {
        AsUpdate?.Invoke();
    }
}