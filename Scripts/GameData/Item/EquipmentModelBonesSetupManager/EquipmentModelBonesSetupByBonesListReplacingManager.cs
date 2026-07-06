using UnityEngine;

namespace MultiplayerARPG
{
    [CreateAssetMenu(fileName = GameDataMenuConsts.EQUIPMENT_MODEL_BONES_SETUP_BY_BONES_LIST_REPLACING_MANAGER_FILE, menuName = GameDataMenuConsts.EQUIPMENT_MODEL_BONES_SETUP_BY_BONES_LIST_REPLACING_MANAGER_MENU, order = GameDataMenuConsts.EQUIPMENT_MODEL_BONES_SETUP_BY_BONES_LIST_REPLACING_MANAGER_ORDER)]
    public class EquipmentModelBonesSetupByBonesListReplacingManager : BaseEquipmentModelBonesSetupManager
    {
        public override void Setup(BaseCharacterModel characterModel, EquipmentModel equipmentModel, GameObject instantiatedObject, BaseEquipmentEntity instantiatedEntity, EquipmentInstantiatedObjectGroup instantiatedObjectGroup, EquipmentContainer equipmentContainer)
        {
            if (GameInstance.Singleton.DimensionType != DimensionType.Dimension3D)
                return;

            SetupForObject(characterModel, instantiatedObject, equipmentContainer);
            if (instantiatedObjectGroup != null && instantiatedObjectGroup.instantiatedObjects != null)
            {
                foreach (GameObject obj in instantiatedObjectGroup.instantiatedObjects)
                {
                    SetupForObject(characterModel, obj, equipmentContainer);
                }
            }
        }

        private void SetupForObject(BaseCharacterModel characterModel, GameObject instantiatedObject, EquipmentContainer equipmentContainer)
        {
            if (instantiatedObject == null)
                return;

            if (!(characterModel is IModelWithSkinnedMeshRenderer skinnedMeshSrc))
            {
                Debug.LogWarning($"[{nameof(EquipmentModelBonesSetupByBonesListReplacingManager)}] Cannot setup bones for \"{instantiatedObject}\", character model \"{characterModel}\" is not a model with animator");
                return;
            }

            SkinnedMeshRenderer defaultSkinnedMesh = null;
            if (equipmentContainer.defaultModel != null)
                defaultSkinnedMesh = equipmentContainer.defaultModel.GetComponentInChildren<SkinnedMeshRenderer>();
            SkinnedMeshRenderer skinnedMeshRenderer = skinnedMeshSrc.SkinnedMeshRenderer;
            if (defaultSkinnedMesh == null && skinnedMeshRenderer == null)
            {
                Debug.LogWarning($"[{nameof(EquipmentModelBonesSetupByBonesListReplacingManager)}] Cannot setup bones for \"{instantiatedObject}\", character model \"{characterModel}\" and equipment container has no skinned mesh renderer");
                return;
            }
            Transform[] bones = null;
            Transform rootBone = null;
            if (defaultSkinnedMesh != null)
            {
                bones = defaultSkinnedMesh.bones;
                rootBone = defaultSkinnedMesh.rootBone;
            }
            else if (skinnedMeshRenderer != null)
            {
                bones = skinnedMeshRenderer.bones;
                rootBone = skinnedMeshRenderer.rootBone;
            }
            SkinnedMeshRenderer[] skinnedMeshes = instantiatedObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < skinnedMeshes.Length; ++i)
            {
                SkinnedMeshRenderer skinnedMesh = skinnedMeshes[i];
                if (skinnedMesh == null)
                    continue;
                skinnedMesh.bones = bones;
                skinnedMesh.rootBone = rootBone;
            }
        }
    }
}
