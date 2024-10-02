using UnityEngine;

public class MineSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _mine;
    [SerializeField] private int _numberOfMineStart;    

    private void Start()
    {
        Spawner.Instance.Spawn(_mine, _numberOfMineStart);
    }
}
