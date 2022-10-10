using UnityEngine;

public class ourButtonInteraction : MonoBehaviour
{
    [SerializeField]
    private Material m_lightOff;
    [SerializeField]
    private Material m_lightOn;
    public bool m_onOff;
    public bool m_toChange;

    [SerializeField]
    private float m_cooldown = 0.5f;
    private float m_timeBeforeClick = 0.0f;

    public float m_cooldownToExtinguish = 0.5f;
    public float m_timerBeforeExtinguish = 0.0f;
    private Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.material = m_lightOff;
        m_onOff = false;
        m_toChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timeBeforeClick > 0.0f)
            m_timeBeforeClick -= Time.deltaTime;
        if (m_timerBeforeExtinguish > 0.0f)
            m_timerBeforeExtinguish -= Time.deltaTime;
        changeMaterial();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (m_timeBeforeClick > 0)
            return;

        m_onOff = !m_onOff;
        m_renderer.material = m_onOff ? m_lightOn : m_lightOff;
        m_timeBeforeClick = m_cooldown;
        m_timerBeforeExtinguish = m_cooldownToExtinguish;

    }


    private void changeMaterial()
    {
        if (m_timerBeforeExtinguish > 0.0f)
            return;
        if (m_toChange == true)
        {
            m_renderer.material = m_onOff ? m_lightOn : m_lightOff;
            m_toChange = false;
        }
    }

}
