using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager Instancia;

    // IMPORTANTE: Asegúrate que esta URL sea exacta a tu carpeta en xampp/htdocs
    private string urlAPI = "http://localhost:8080/Juego/game.php";

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    public void GuardarPartida(GameData datos)
    {
        StartCoroutine(EnviarPost(datos));
    }

    private IEnumerator EnviarPost(GameData datos)
    {
        string json = JsonUtility.ToJson(datos);
        using (UnityWebRequest request = new UnityWebRequest(urlAPI, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log("Partida guardada: " + request.downloadHandler.text);
            else
                Debug.LogError("Error al guardar: " + request.error);
        }
    }

    public IEnumerator CargarPartida(string id, System.Action<GameData> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(urlAPI + "?jugador_id=" + id))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                GameData datos = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
                callback(datos);
            }
            else
            {
                Debug.LogError("Error al cargar: " + request.error);
                callback(null);
            }
        }
    }
}