using UnityEngine;

public class SlotSelected : MonoBehaviour
{
    [SerializeField] private Module module;
    
    public void SetModule(Module m)
    {
        module = m;
    }

    public Module GetModule()
        { return module; }

}
