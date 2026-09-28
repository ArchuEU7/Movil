using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterCreator : MonoBehaviour
{
    [System.Serializable]
    public class  CharacterData
    {
        public string nombre;
        public int edad;
    }
    //Botones
    [SerializeField] Button Añadir; 
    [SerializeField] GameObject Boton1;

    //Nombres
    [SerializeField] GameObject NombreTexto;
    [SerializeField] TMP_InputField NombreUsuario;

    //Edad
    [SerializeField] GameObject EdadTexto;
    [SerializeField] TMP_InputField EdadUsuario;

    public List<CharacterData> personajes = new List<CharacterData>();
    string nombreActual;

    void Awake()
    {
        Añadir.onClick.AddListener(AñadirPersonaje);
        NombreUsuario.onSubmit.AddListener(AlEnviarNombre);
        EdadUsuario.onSubmit.AddListener(AlEnviarEdad);

        OcultarTodo();
    }

     void OnDestroy()
    {
        Añadir.onClick.RemoveListener(AñadirPersonaje);
        NombreUsuario.onSubmit.RemoveListener(AlEnviarNombre);
        EdadUsuario.onSubmit.RemoveListener(AlEnviarEdad);
    }

    //Paso 1 ok
    void AñadirPersonaje()
    {
        OcultarTodo();
        Boton1.SetActive(true);

        NombreTexto.SetActive(true);
        NombreUsuario.gameObject.SetActive(true);
        NombreUsuario.text = "";
        NombreUsuario.ActivateInputField();
    }

    //Paso 2 ok
    void AlEnviarNombre(string texto)
    {
        texto = texto.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            NombreUsuario.ActivateInputField();
            return;
        }

        nombreActual = texto;
        NombreUsuario.gameObject.SetActive(false);
        NombreTexto.SetActive(false);
        EdadTexto.SetActive(true);
        EdadUsuario.gameObject.SetActive(true);
        EdadUsuario.text = "";
        EdadUsuario.ActivateInputField();
    }

    //Paso 3 ok
    void AlEnviarEdad(string texto)
    {
       if (!int.TryParse(texto, out int edad) || edad < 0)
        {
            EdadUsuario.ActivateInputField();
            return;
        }

        personajes.Add(new CharacterData { nombre = nombreActual, edad = edad });
        Debug.Log($"Personaje guardado: {nombreActual}, {edad} años");

        OcultarTodo();
    }

    void OcultarTodo()
    {
        Boton1.SetActive(false);
        NombreTexto.SetActive(false);
        NombreUsuario.gameObject.SetActive(false);
        EdadTexto.SetActive(false);
        EdadUsuario.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
