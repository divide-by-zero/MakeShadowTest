using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour{
	private Transform _transform;
	public Transform transform {
		get { return _transform ?? (_transform = GetComponent<Transform>()); }
	}

	
	private Animation _animation;
	private Renderer _renderer;
	private Rigidbody _rigidbody;
	private Rigidbody2D _rigidbody2D;
	private SpriteRenderer _spriteRenderer;

	public Rigidbody Rigidbody{
		get { return _rigidbody ?? (_rigidbody = GetComponent<Rigidbody>()); }
	}

	public Rigidbody2D Rigidbody2D{
		get { return _rigidbody2D ?? (_rigidbody2D = GetComponent<Rigidbody2D>()); }
	}

	public Renderer Renderer{
		get { return _renderer ?? (_renderer = GetComponent<Renderer>()); }
	}

	public SpriteRenderer SpriteRenderer{
		get { return _spriteRenderer ?? (_spriteRenderer = GetComponent<SpriteRenderer>()); }
	}

	public Animation Animation{
		get { return _animation ?? (_animation = GetComponent<Animation>()); }
	}

	protected virtual void Awake(){
	}

	protected virtual void FixedUpdate(){
	}

	protected virtual void LateUpdate(){
	}

	protected virtual void OnAnimatorIK(int layerIndex){
	}

	protected virtual void OnAnimatorMove(){
	}

	protected virtual void OnApplicationFocus(bool focus){
	}

	protected virtual void OnApplicationPause(bool pause){
	}

	protected virtual void OnApplicationQuit(){
	}

	protected virtual void OnBecameInvisible(){
	}

	protected virtual void OnBecameVisible(){
	}

	protected virtual void OnCollisionEnter(Collision collision){
	}

	protected virtual void OnCollisionEnter2D(Collision2D coll){
	}

	protected virtual void OnCollisionExit(Collision collision){
	}

	protected virtual void OnCollisionExit2D(Collision2D coll){
	}

	protected virtual void OnTriggerEnter(Collider collisionInfo){
	}

	protected virtual void OnTriggerEnter2D(Collision2D coll){
	}

	protected virtual void OnConnectedToServer(){
	}

	protected virtual void OnControllerColliderHit(ControllerColliderHit hit){
	}

	protected virtual void OnDestroy(){
	}

	protected virtual void OnDisable(){
	}

	protected virtual void OnDrawGizmos(){
	}

	protected virtual void OnDrawGizmosSelected(){
	}

	protected virtual void OnEnable(){
	}

	protected virtual void OnGUI(){
	}

	protected virtual void OnJointBreak(float breakForce){
	}

	protected virtual void OnLevelWasLoaded(int level){
	}

	protected virtual void OnMouseDown(){
	}

	protected virtual void OnMouseDrag(){
	}

	protected virtual void OnMouseEnter(){
	}

	protected virtual void OnMouseExit(){
	}

	protected virtual void OnMouseOver(){
	}

	protected virtual void OnMouseUp(){
	}

	protected virtual void OnMouseUpAsButton(){
	}

	protected virtual void OnParticleCollision(GameObject other){
	}

	protected virtual void OnPostRender(){
	}

	protected virtual void OnPreCull(){
	}

	protected virtual void OnPreRender(){
	}

	protected virtual void OnRenderImage(RenderTexture destination, RenderTexture source){
	}

	protected virtual void OnRenderObject(){
	}

	protected virtual void OnServerInitialized(){
	}

	protected virtual void OnCollisionStay(Collision other){
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
	}

	protected virtual void OnTriggerExit(Collider other){
	}

	protected virtual void OnTriggerExit2D(Collider2D other){
	}

	protected virtual void OnTriggerStay(Collider other){
	}

	protected virtual void OnTriggerStay2D(Collider2D other){
	}

	protected virtual void OnValidate(){
	}

	protected virtual void OnWillRenderObject(){
	}

	protected virtual void Reset(){
	}

	protected virtual void Start(){
	}

	protected virtual void Update(){
	}
}

public static class TransformExtension{
	private static Vector3 work;

	public static Transform SetPosition(this Transform t, float? x = null, float? y = null, float? z = null){
		work = t.transform.localPosition;
		if (x.HasValue) work.x = x.Value;
		if (y.HasValue) work.y = y.Value;
		if (z.HasValue) work.z = z.Value;
		t.transform.localPosition = work;
		return t;
	}

	public static Transform AddPosition(this Transform t, float x = 0, float y = 0, float z = 0){
		work = t.transform.localPosition;
		work.x += x;
		work.y += y;
		work.z += z;
		t.transform.localPosition = work;
		return t;
	}

	public static Transform AddPosition(this Transform t, Vector2 add){
		return AddPosition(t, add.x, add.y);
	}

	public static Transform AddPosition(this Transform t, Vector3 add){
		return AddPosition(t, add.x, add.y, add.z);
	}

	public static Transform SetAngle(this Transform t, float? x = null, float? y = null, float? z = null){
		work = t.transform.localRotation.eulerAngles;
		if (x.HasValue) work.x = x.Value;
		if (y.HasValue) work.y = y.Value;
		if (z.HasValue) work.z = z.Value;
		t.transform.localRotation = Quaternion.Euler(work);
		return t;
	}

	public static Transform AddAngle(this Transform t, float x = 0, float y = 0, float z = 0){
		work = t.transform.localRotation.eulerAngles;
		work.x += x;
		work.y += y;
		work.z += z;
		t.transform.localRotation = Quaternion.Euler(work);
		return t;
	}

	public static Transform AddAngle(this Transform t, Vector2 add){
		return AddAngle(t, add.x, add.y);
	}

	public static Transform AddAngle(this Transform t, Vector3 add){
		return AddAngle(t, add.x, add.y, add.z);
	}

	public static Transform SetScale(this Transform t, float? x = null, float? y = null, float? z = null){
		work = t.transform.localScale;
		if (x.HasValue) work.x = x.Value;
		if (y.HasValue) work.y = y.Value;
		if (z.HasValue) work.z = z.Value;
		t.transform.localScale = work;
		return t;
	}

	public static Transform AddScale(this Transform t, float x = 0, float y = 0, float z = 0){
		work = t.transform.localScale;
		work.x += x;
		work.y += y;
		work.z += z;
		t.transform.localScale = work;
		return t;
	}

	public static Transform AddScale(this Transform t, Vector2 add){
		return AddScale(t, add.x, add.y);
	}

	public static Transform AddScale(this Transform t, Vector3 add){
		return AddScale(t, add.x, add.y, add.z);
	}
}