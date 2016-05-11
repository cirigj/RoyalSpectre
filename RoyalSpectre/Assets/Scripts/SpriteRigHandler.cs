using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteRigHandler : MonoBehaviour {

    [System.Serializable]
    public class BoneSpriteBinder {

        public enum BoneType {
            Root = 0,
            Hips = 1,
            Chest = 2,
            Head = 3,
            LeftUpperArm = 4,
            LeftForeArm = 5,
            RightUpperArm = 6,
            RightForeArm = 7,
            LeftThigh = 8,
            LeftShin = 9,
            RightThigh = 10,
            RightShin = 11,
        }

        public BoneType boneType;

        public Transform runtimeBone;
        public SpriteRenderer cosmeticBone;
        public Sprite sprite;

        public void Init() {
            if (cosmeticBone == null) { return; }

            cosmeticBone.sprite = sprite;
        }

        public void SetVisiblityInHierarchy(bool isVisible) {
            if (isVisible) {
                cosmeticBone.hideFlags = HideFlags.None;
            }
            else {
                cosmeticBone.hideFlags = HideFlags.HideInHierarchy;
            }
        }
    }

    [HideInInspector]
    public bool _initializeRigAtStartup = false;

    [HideInInspector]
    public bool _showBones = false;

    [HideInInspector]
    public bool _showCosmeticSprites = false;

    public List<BoneSpriteBinder> boneList;

    public Color boneColor = Color.cyan;
    public Color jointColor = Color.yellow;
    public float jointRadius = 0.05f;

    void Awake() {
        InitRig();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitRig() {

        foreach (BoneSpriteBinder binder in boneList) {
            binder.Init();
        }
    }

    public void SetViewCosmeticBonesInHierarchy(bool showBones) {

        foreach (BoneSpriteBinder binder in boneList) {
            binder.SetVisiblityInHierarchy(showBones);
        }
    }

    void OnDrawGizmos() {
        if (_showBones) {
            Gizmo_ShowBones();

            SceneView.RepaintAll();
        }
    }

    private void Gizmo_ShowBones() {
        Color originalColor = Gizmos.color;

        foreach (BoneSpriteBinder binder in boneList) {
            Gizmos.color = jointColor;
            Gizmos.DrawSphere(binder.runtimeBone.position, jointRadius);

            Gizmos.color = boneColor;
            foreach (Transform child in binder.runtimeBone) {
                if (boneList.Select(x => x.runtimeBone).ToList<Transform>().Contains(child)) {
                    Gizmos.DrawLine(binder.runtimeBone.position, child.position);
                }
            }
        }

        Gizmos.color = originalColor;
    }

}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(SpriteRigHandler))]
public class SpriteRigHandler_Editor : Editor {

    SpriteRigHandler selfScript;

    void OnEnable() {
        selfScript = (SpriteRigHandler)target;
    }
    
    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Apply Rig Changes")) {
            selfScript.InitRig();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();


        selfScript._showBones = GUILayout.Toggle(selfScript._showBones, "Show Bones", EditorStyles.radioButton);
        //selfScript._showCosmeticSprites = GUILayout.Toggle(selfScript._showCosmeticSprites, "Show Cosmetic Sprites", EditorStyles.radioButton);
        if (GUILayout.Button("Show Cosmetic Bones")) {
            selfScript._showCosmeticSprites = !selfScript._showCosmeticSprites;
            selfScript.SetViewCosmeticBonesInHierarchy(selfScript._showCosmeticSprites);
        }

        EditorGUILayout.EndVertical();


        EditorGUILayout.EndVertical();

    }
    
}
#endif
