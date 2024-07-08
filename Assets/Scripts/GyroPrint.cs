using UnityEngine;

public class GyroChart : MonoBehaviour
{
    private Gyroscope m_Gyro;

    // Valores para o gráfico
    private float[] gyroRotationRates;
    private Vector3[] gyroAttitudes;
    private Vector3[] accelValues;

    // Parâmetros do gráfico
    private float chartWidth = 400f;
    private float chartHeight = 200f;
    private float chartPadding = 20f;

    void Start()
    {
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;

        gyroRotationRates = new float[100];
        gyroAttitudes = new Vector3[100];
        accelValues = new Vector3[100];
    }

    void Update()
    {
        // Atualizar os valores dos gráficos
        for (int i = 99; i > 0; i--)
        {
            gyroRotationRates[i] = gyroRotationRates[i - 1];
            gyroAttitudes[i] = gyroAttitudes[i - 1];
            accelValues[i] = accelValues[i - 1];
        }
        gyroRotationRates[0] = m_Gyro.rotationRate.magnitude;
        gyroAttitudes[0] = m_Gyro.attitude.eulerAngles;
        accelValues[0] = m_Gyro.userAcceleration;
    }

    void OnGUI()
    {
        // Desenhar o gráfico de rotação do giroscópio
        DrawChart(new Rect(50, 50, chartWidth, chartHeight), gyroRotationRates, Color.red);

        // Desenhar o gráfico de atitude do giroscópio
        DrawChart(new Rect(50, 300, chartWidth, chartHeight), gyroAttitudes, Color.blue);

        // Desenhar o gráfico de aceleração do dispositivo
        DrawChart(new Rect(50, 550, chartWidth, chartHeight), accelValues, Color.green);
    }

    void DrawChart(Rect area, float[] values, Color color)
    {
        // Desenhar o fundo do gráfico
        GUI.Box(area, "");

        // Desenhar a linha do gráfico
        float maxValue = Mathf.Max(values);
        float minValue = Mathf.Min(values);
        float range = maxValue - minValue;

        Vector2 prevPoint = Vector2.zero;
        for (int i = 0; i < values.Length; i++)
        {
            float x = area.x + chartPadding + (i * (area.width - 2 * chartPadding) / (values.Length - 1));
            float y = area.y + area.height - chartPadding - ((values[i] - minValue) / range) * (area.height - 2 * chartPadding);
            Vector2 point = new Vector2(x, y);

            if (i > 0)
            {
                // Desenhar a linha entre os pontos
                DrawLine(prevPoint, point, color, 2f);
            }

            prevPoint = point;
        }
    }

    void DrawLine(Vector2 start, Vector2 end, Color color, float width)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        GUI.skin.box.normal.background = texture;
        GUI.Box(new Rect(start.x, start.y, end.x - start.x, end.y - start.y), GUIContent.none);
    }

    void DrawChart(Rect area, Vector3[] values, Color color)
    {
        // Converter os valores Vector3 para float
        float[] floatValues = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            floatValues[i] = values[i].magnitude;
        }

        // Chamar o método DrawChart para desenhar o gráfico
        DrawChart(area, floatValues, color);
    }
}
