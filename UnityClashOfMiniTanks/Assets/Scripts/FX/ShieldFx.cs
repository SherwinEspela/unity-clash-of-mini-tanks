using UnityEngine;

public class ShieldFx : CmtFxScript
{
    [SerializeField] GameObject sphereOuter;
    [SerializeField] GameObject sphereInner;

    [SerializeField] protected float rotateSpeed = 300.0f;
    [SerializeField] float fullScaleValue_SphereOuter = 2.5f;
    [SerializeField] float fullScaleValue_SphereInner = 2.4f;

    private Renderer sphereInnerRenderer;
    private Vector3 sphereOuterFullScale;
    private Vector3 sphereInnerFullScale;

    // event
    public event CmtFxEvent OnShieldFxHidden;

    private void Init()
    {
        if (!this.sphereInnerRenderer)
        {
            this.sphereInnerRenderer = this.sphereInner.GetComponent<Renderer>();
        }

        this.sphereOuter.transform.localScale = Vector3.zero;
        this.sphereInner.transform.localScale = Vector3.zero;

        sphereOuterFullScale = new Vector3(fullScaleValue_SphereOuter, fullScaleValue_SphereOuter, fullScaleValue_SphereOuter);
        sphereInnerFullScale = new Vector3(fullScaleValue_SphereInner, fullScaleValue_SphereInner, fullScaleValue_SphereInner);

        this.sphereInnerRenderer.material.SetTextureOffset("_MainTex", new Vector2(1, 1));
    }

    public override void Display()
    {
        Init();
        base.Display();
    }

    void Update()
    {
        if (this.isEnabled)
        {
            sphereOuter.transform.Rotate(new Vector3(0, this.rotateSpeed * Time.deltaTime, 0));
            this.sphereInnerRenderer.material.SetTextureOffset("_MainTex", new Vector2(1, this.offsetScrollSpeed * Time.time));

            if (isDisplaying)
            {
                this.sphereOuter.transform.localScale = Vector3.Lerp(this.sphereOuter.transform.localScale, sphereOuterFullScale, Time.deltaTime * this.scaleTime);
                this.sphereInner.transform.localScale = Vector3.Lerp(this.sphereInner.transform.localScale, sphereInnerFullScale, Time.deltaTime * this.scaleTime);
            } else
            {
                this.sphereOuter.transform.localScale = Vector3.Lerp(this.sphereOuter.transform.localScale, Vector3.zero, Time.deltaTime * this.scaleTime);
                this.sphereInner.transform.localScale = Vector3.Lerp(this.sphereInner.transform.localScale, Vector3.zero, Time.deltaTime * this.scaleTime);

                if (this.sphereOuter.transform.localScale.x <= 1.0f)
                {
                    if (OnShieldFxHidden != null)
                    {
                        OnShieldFxHidden();
                    }
                    this.isEnabled = false;
                }
            }
        }
    }
}
