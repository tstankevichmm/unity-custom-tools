#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace CustomTools.AnimationEvents
{
    [CustomEditor(typeof(AnimationEventStateBehaviour), true)]
    public class AnimationEventStateBehaviourEditor : Editor
    {
        private AnimationClip _previewClip;
        private bool _isPreviewing;

        [MenuItem("GameObject/Enforce T-Pose", false, 0)]
        public static void EnforceTPose()
        {
            GameObject selected = Selection.activeGameObject;

            if (!selected || !selected.TryGetComponent(out Animator animator) || !animator.avatar)
                return;

            SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

            foreach (HumanBodyBones humanBodyBones in Enum.GetValues(typeof(HumanBodyBones)))
            {
                if(humanBodyBones == HumanBodyBones.LastBone)
                    continue;

                Transform boneTransform = animator.GetBoneTransform(humanBodyBones);
                if(!boneTransform)
                    continue;

                SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
                
                if(skeletonBone.name == null)
                    continue;

                if (humanBodyBones == HumanBodyBones.Hips)
                    boneTransform.localPosition = skeletonBone.position;

                boneTransform.localRotation = skeletonBone.rotation;
            }
            
            Debug.Log($"T-Pose Enforced on {selected.name}");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimationEventStateBehaviour stateBehaviour = (AnimationEventStateBehaviour)target;

            if (Validate(stateBehaviour, out _previewClip, out string errorMessage))
            {
                if (_isPreviewing)
                {
                    if (GUILayout.Button("Stop Preview"))
                    {
                        EnforceTPose();
                        _isPreviewing = false;
                    }
                    else
                    {
                        float previewTime = PreviewAnimationClip(_previewClip, stateBehaviour);
                        GUILayout.Label($"Previewing at {previewTime:F2}s", EditorStyles.helpBox);   
                    }
                }
                else if (GUILayout.Button("Preview in Scene"))
                {
                        _isPreviewing = true;
                }
            }
            else
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
        }

        private float PreviewAnimationClip(AnimationClip clip, AnimationEventStateBehaviour stateBehaviour)
        {
            if (clip == null)
                return -1f;

            float previewTime = stateBehaviour.GetTriggerTime() * clip.length;
            AnimationMode.StartAnimationMode();
            GUILayout.Label($"GO: {Selection.activeGameObject.name} | Clip: {clip.name} | Time: {previewTime}", EditorStyles.helpBox);
            AnimationMode.SampleAnimationClip(Selection.activeGameObject, clip, previewTime);
            //AnimationMode.StopAnimationMode();

            return previewTime;
        }

        private bool Validate(AnimationEventStateBehaviour stateBehaviour, out AnimationClip clip, out string errorMessage)
        {
            AnimatorController controller = GetValidAnimatorController(out errorMessage);
            clip = null;

            if (controller == null)
                return false;

            ChildAnimatorState matchingState = controller.layers
                .SelectMany(layer => layer.stateMachine.states)
                .FirstOrDefault(state => state.state.behaviours.Contains(stateBehaviour));

            if (matchingState.state == null)
            {
                errorMessage = "The state in ChildAnimatorState is null.";
                return false;
            }
            
            clip = matchingState.state?.motion as AnimationClip;

            if (clip == null)
            {
                errorMessage = $"No valid AnimationClip found for the current state {matchingState.state.name}";
                return false;
            }

            return true;
        }

        private AnimatorController GetValidAnimatorController(out string errorMessage)
        {
            errorMessage = string.Empty;

            GameObject targetGO = Selection.activeGameObject;

            if (targetGO == null)
            {
                errorMessage = "Please select a GameObject with an Animator component to preview";
                return null;
            }

            Animator animator = targetGO.GetComponent<Animator>();
            if (animator == null)
            {
                errorMessage = "The selected GameObject does not have an Animator component";
                return null;
            }
            
            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null)
            {
                errorMessage = "The selected Animator is not a valid AnimationController.";
                return null;
            }
            
            return animatorController;
        }
    }
}
#endif