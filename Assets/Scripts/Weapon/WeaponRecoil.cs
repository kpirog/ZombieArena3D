using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] private Transform recoilTransform;

    [SerializeField] private float kickBackForce = -1f;
    [SerializeField] private float recoilReturnTime = 10f;
    [SerializeField] private float finalRecoilTime = 20f;
    
    private float finalRecoil;
    private float currentRecoil;
    private AimStateManager aimStateManager;

    private void Start()
    {
        aimStateManager = GetComponentInParent<AimStateManager>();
        recoilTransform = aimStateManager.recoilTransform;
    }

    private void Update()
    {
        currentRecoil = Mathf.Lerp(currentRecoil, 0f, recoilReturnTime * Time.deltaTime);
        finalRecoil = Mathf.Lerp(finalRecoil, currentRecoil, finalRecoilTime * Time.deltaTime);

        recoilTransform.localPosition = new Vector3(recoilTransform.localPosition.x, recoilTransform.localPosition.y, finalRecoil);
    }
    public void TriggerRecoil() => currentRecoil += kickBackForce;
}
