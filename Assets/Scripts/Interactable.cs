using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private ItemBase itemBase;
    [SerializeField] private ParticleSystem rarityParticle;
    [SerializeField] private float pickUpDistance = 2f;

    protected bool isCollected = false;
    protected bool isInEquipment;

    protected ActionStateManager action;
    protected Item3DTooltip tooltip;

    protected EquipmentUI equipmentUI;
    [HideInInspector] public Animator anim;
    public ItemBase ItemBase => itemBase;
    public virtual bool IsInEquipment
    {
        get => isInEquipment;
        set
        {
            SetRarityParticle(value);
            isInEquipment = value;
            anim.SetBool("IsInEquipment", value);

            anim.enabled = !value;
        }
    }
    protected virtual void Awake()
    {
        action = FindObjectOfType<ActionStateManager>();
        equipmentUI = FindObjectOfType<EquipmentUI>();
        tooltip = FindObjectOfType<Item3DTooltip>();
        anim = GetComponent<Animator>();
        SetRarityParticle(IsInEquipment);
    }
    private void Start()
    {
        anim.SetBool("IsInEquipment", IsInEquipment);
    }
    private void Update()
    {
        if (!IsInEquipment)
        {
            if (Vector3.Distance(action.transform.position, transform.position) <= 2f && action.pickUpAction.triggered && !action.CanPickUp)
            {
                action.CanPickUp = true;
                isCollected = true;
            }

            if (isCollected)
            {
                PickUp();
            }
        }
    }
    protected virtual void PickUp()
    {
        equipmentUI.AddItem(ItemBase);
        Destroy(gameObject);
    }
    private void OnMouseEnter()
    {
        tooltip.transform.position = new Vector3(transform.position.x, transform.position.y + tooltip.yOffset, transform.position.z);
        tooltip.SetLookRotation(Camera.main.transform);
        tooltip.DisplayTooltip(ItemBase);
    }
    private void OnMouseExit()
    {
        tooltip.HideTooltip();
    }
    private void OnDestroy()
    {
        if (Application.isFocused)
            action.CanPickUp = false;
    }
    protected void SetRarityParticle(bool isStopped)
    {
        rarityParticle.gameObject.SetActive(!isStopped);

        ParticleSystem.MainModule main = rarityParticle.main;
        Color rarityColor = new Color(itemBase.ItemRarity.color.r, itemBase.ItemRarity.color.g, itemBase.ItemRarity.color.b, 0.2f);
        main.startColor = rarityColor;
    }
}
