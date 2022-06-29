using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnManager : MonoSingleton<ObjectSpawnManager>
{
    [SerializeField] private List<string> spawnableObjectTypes;

    #region PrivateVariables;
    private ConstantVariables _constantVariables;
    #endregion
    private void Awake()
    {
        Setup();
    }
    public ObjectBase SpawnObjectAndSetPosition(int stackCount)
    {
        if ((LevelManager.Singleton.PlayerTransform.position + new Vector3(0, 0, _constantVariables.NewObjectHolderPosZInterval)).z < LevelManager.Singleton.FinishLine.position.z)
        {
            Transform newObjectHolder = ObjectPool.Singleton.GetObject("pizzaBoxHolder", 0).transform;
            newObjectHolder.position = LevelManager.Singleton.PlayerTransform.position + new Vector3(0, 0, _constantVariables.NewObjectHolderPosZInterval);
            newObjectHolder.SetParent(null);

            ObjectBase tempBase = newObjectHolder.GetComponent<ObjectBase>();
            PrepareObjectHolder(tempBase, stackCount);
            return tempBase;
        }
        else
        {
            return null;
        }
    }
    private void PrepareObjectHolder(ObjectBase objectBase, int stackCount)
    {
        objectBase.SetObjects(spawnableObjectTypes[Random.Range(0,spawnableObjectTypes.Count)], 0, stackCount);
    }
    private void Setup()
    {
        _constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
}
