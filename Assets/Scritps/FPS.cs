using UnityEngine;

public class FPS: MonoBehaviour
{
    private float deltaTime = 0.0f;

    // Update вызывается каждый кадр
    void Update()
    {
        // Вычисляем разницу во времени между кадрами
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    // Используем OnGUI для отображения FPS на экране
    void OnGUI()
    {
        // Вычисляем FPS
        float fps = 1.0f / deltaTime;

        // Отображаем FPS в верхнем левом углу экрана
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;
        
        // Отображаем FPS
        GUI.Label(new Rect(10, 10, 200, 40), "FPS: " + Mathf.Ceil(fps).ToString(), style);
    }
}