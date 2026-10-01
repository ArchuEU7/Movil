using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public int id;
        public string nombre;
        public int edad;
    }

    [System.Serializable]
    class SaveData
    {
        public List<CharacterData> personajes = new List<CharacterData>();
        public int siguienteId = 1;
    }

    //Botones
    [SerializeField] Button botonAnadir;
    [SerializeField] GameObject boton1;

    //Nombre
    [SerializeField] GameObject nombreTexto;
    [SerializeField] TMP_InputField nombreUsuario;

    //Edad
    [SerializeField] GameObject edadTexto;
    [SerializeField] TMP_InputField edadUsuario;

    //Busqueda
    [SerializeField] Button botonBuscar;
    [SerializeField] GameObject panelBusqueda;
    [SerializeField] TMP_InputField idUsuario;
    [SerializeField] TextMeshProUGUI resultadoTexto;

    public List<CharacterData> personajes = new List<CharacterData>();
    int siguienteId = 1;

    string nombreActual;
    string RutaArchivo => Path.Combine(Application.persistentDataPath, "personajes.json");

    void Awake()
    {
        botonAnadir.onClick.AddListener(EmpezarCreacion);
        nombreUsuario.onSubmit.AddListener(AlEnviarNombre);
        edadUsuario.onSubmit.AddListener(AlEnviarEdad);

        botonBuscar.onClick.AddListener(AbrirBusqueda);
        idUsuario.onSubmit.AddListener(AlBuscarId);

        OcultarTodo();
        Cargar();
    }

    void OnDestroy()
    {
        botonAnadir.onClick.RemoveListener(EmpezarCreacion);
        nombreUsuario.onSubmit.RemoveListener(AlEnviarNombre);
        edadUsuario.onSubmit.RemoveListener(AlEnviarEdad);

        botonBuscar.onClick.RemoveListener(AbrirBusqueda);
        idUsuario.onSubmit.RemoveListener(AlBuscarId);
    }

    //Crear pj

    void EmpezarCreacion()
    {
        OcultarTodo();
        boton1.SetActive(true);

        nombreTexto.SetActive(true);
        nombreUsuario.gameObject.SetActive(true);
        nombreUsuario.text = "";
        nombreUsuario.ActivateInputField();
    }

    void AlEnviarNombre(string texto)
    {
        texto = texto.Trim();

        if (string.IsNullOrEmpty(texto))
        {
            nombreUsuario.ActivateInputField();
            return;
        }

        nombreActual = texto;

        nombreUsuario.gameObject.SetActive(false);
        nombreTexto.SetActive(false);

        edadTexto.SetActive(true);
        edadUsuario.gameObject.SetActive(true);
        edadUsuario.text = "";
        edadUsuario.ActivateInputField();
    }

    void AlEnviarEdad(string texto)
    {
        if (!int.TryParse(texto, out int edad) || edad < 0)
        {
            edadUsuario.ActivateInputField();
            return;
        }

        var nuevo = new CharacterData
        {
            id = siguienteId,
            nombre = nombreActual,
            edad = edad
        };
        siguienteId++;

        personajes.Add(nuevo);
        Guardar();

        Debug.Log($"Personaje creado: #{nuevo.id} - {nuevo.nombre}, {nuevo.edad} años");

        OcultarTodo();
    }

    //Buscar personaje

    void AbrirBusqueda()
    {
        OcultarTodo();
        panelBusqueda.SetActive(true);
        idUsuario.text = "";
        resultadoTexto.text = "";
        idUsuario.ActivateInputField();
    }

    void AlBuscarId(string texto)
    {
        if (!int.TryParse(texto, out int id))
        {
            resultadoTexto.text = "ID no válido.";
            idUsuario.ActivateInputField();
            return;
        }

        var encontrado = personajes.Find(p => p.id == id);

        resultadoTexto.text = encontrado != null
            ? $"#{encontrado.id} - {encontrado.nombre}, {encontrado.edad} años"
            : $"No se encontró el ID {id}.";

        idUsuario.ActivateInputField();
    }

    //Guardado

    void Guardar()
    {
        try
        {
            var datos = new SaveData { personajes = personajes, siguienteId = siguienteId };
            string json = JsonUtility.ToJson(datos, true);
            File.WriteAllText(RutaArchivo, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar: " + e.Message);
        }
    }

    void Cargar()
    {
        if (!File.Exists(RutaArchivo)) return;

        try
        {
            string json = File.ReadAllText(RutaArchivo);
            var datos = JsonUtility.FromJson<SaveData>(json);
            if (datos != null)
            {
                personajes = datos.personajes ?? new List<CharacterData>();
                siguienteId = datos.siguienteId > 0 ? datos.siguienteId : 1;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar: " + e.Message);
        }
    }

    void OcultarTodo()
    {
        boton1.SetActive(false);
        nombreTexto.SetActive(false);
        nombreUsuario.gameObject.SetActive(false);
        edadTexto.SetActive(false);
        edadUsuario.gameObject.SetActive(false);

        panelBusqueda.SetActive(false);
    }
}