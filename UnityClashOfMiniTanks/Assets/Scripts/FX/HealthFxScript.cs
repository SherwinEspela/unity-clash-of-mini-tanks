using System.Collections;
using UnityEngine;

public class HealthFxScript : CmtFxScript
{
    [SerializeField] GameObject sphereOuter;
    [SerializeField] GameObject sphereInner;

    [SerializeField] protected float rotateSpeedSphereOuter = 500.0f;
    [SerializeField] protected float rotateSpeedSphereInner = 100.0f;
    [SerializeField] float fullScaleValue_SphereOuter = 7f;

    private Renderer sphereInnerRenderer;
    private float sphereInnerBoost = 0;
    [SerializeField] float sphereInnerBoostScaleTime = 2f;
    [SerializeField] float sphereInnerBoostMax = 1.0f;

    private Renderer sphereOuterRenderer;
    private float sphereOuterBoost = 0;
    [SerializeField] float sphereOuterBoostScaleTime = 2f;
    [SerializeField] float sphereOuterVanishingTime = 2f;
    [SerializeField] float sphereOuterBoostMax = 1.0f;

    private bool shouldDisplayInnerSphere = false;
    private bool shouldDisplayOuterSphere = false;
    private Vector3 sphereOuterFullScale;

    [SerializeField] float hideTime = 2.5f;

    // event
    public event CmtFxEvent OnHealthFxHidden;

    private void Init()
    {
        if (!this.sphereInnerRenderer)
        {
            this.sphereInnerRenderer = this.sphereInner.GetComponent<Renderer>();
        }

        if (!this.sphereOuterRenderer)
        {
            this.sphereOuterRenderer = this.sphereOuter.GetComponent<Renderer>();
        }

        this.sphereOuter.transform.localScale = Vector3.zero;
        this.sphereOuterFullScale = new Vector3(fullScaleValue_SphereOuter, fullScaleValue_SphereOuter, fullScaleValue_SphereOuter);
  
        this.shouldDisplayInnerSphere = false;
        this.shouldDisplayOuterSphere = false;
        sphereInnerBoost = 0f;
        sphereOuterBoost = 0f;
        this.sphereInnerRenderer.material.SetFloat("_Boost", sphereInnerBoost);
        this.sphereOuterRenderer.material.SetFloat("_Boost", sphereOuterBoost);
    }

    public override void Display()
    {
        Init();
        base.Display();

        shouldDisplayInnerSphere = true;
        StartCoroutine(HideInnerSphereCoroutine());
    }

    void Update()
    {
        if (this.isEnabled)
        {
            sphereOuter.transform.Rotate(new Vector3(0, this.rotateSpeedSphereOuter * Time.deltaTime, 0));
            sphereInner.transform.Rotate(new Vector3(0, this.rotateSpeedSphereInner * Time.deltaTime, 0));
            this.sphereInnerRenderer.material.SetTextureOffset("_MainTex", new Vector2(1, this.offsetScrollSpeed * Time.time));

            if (isDisplaying)
            {
                if (shouldDisplayInnerSphere)
                {
                    sphereInnerBoost = Mathf.Lerp(sphereInnerBoost, sphereInnerBoostMax, Time.deltaTime * sphereInnerBoostScaleTime);
                    this.sphereInnerRenderer.material.SetFloat("_Boost", sphereInnerBoost);
                } else
                {
                    sphereInnerBoost = Mathf.Lerp(sphereInnerBoost, 0.0f, Time.deltaTime * sphereInnerBoostScaleTime);
                    this.sphereInnerRenderer.material.SetFloat("_Boost", sphereInnerBoost);
                }

                this.sphereOuter.transform.localScale = Vector3.Lerp(this.sphereOuter.transform.localScale, sphereOuterFullScale, Time.deltaTime * this.scaleTime);

                if (this.sphereOuter.transform.localScale.x >= (sphereOuterFullScale.x / 2.0f) + 1.0f)
                {
                    sphereOuterBoost = Mathf.Lerp(sphereOuterBoost, 0.0f, Time.deltaTime * sphereOuterVanishingTime);
                    this.sphereOuterRenderer.material.SetFloat("_Boost", sphereOuterBoost);
                } else
                {
                    sphereOuterBoost = Mathf.Lerp(sphereOuterBoost, sphereOuterBoostMax, Time.deltaTime * sphereOuterBoostScaleTime);
                    this.sphereOuterRenderer.material.SetFloat("_Boost", sphereOuterBoost);
                }   
            }
        }
    }

    IEnumerator HideInnerSphereCoroutine()
    {
        yield return new WaitForSeconds(0.75f);
        shouldDisplayInnerSphere = false;
        StartCoroutine(DisableFxCoroutine());
    }

    IEnumerator DisableFxCoroutine()
    {
        yield return new WaitForSeconds(hideTime);
        
        if (OnHealthFxHidden != null)
        {
            OnHealthFxHidden();
        }

        this.isEnabled = false;
    }
}
