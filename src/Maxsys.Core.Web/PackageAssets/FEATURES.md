# Maxsys.Core.Web
Biblioteca Maxsys Core para utilização de recursos Web.

## Attributes
- `TitledActionAttribute`
    - Utilizado para definir um Título para uma Action
    <details>
        <summary>Exemplo de uso</summary>

    ```cs
    [TitledAction("Titled_Action")]
    public IActionResult Get() { ... }
    ```
    </details>

- `FromJsonAttribute`
    - Usado em Controller Actions. ModelBinderAttribute que recebe um JSON string `FromQuery` e converte no objeto de destino.
    <details>
        <summary>Exemplo de uso</summary>

    #### Objeto:
    ```cs
    
    public class Sample
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Items { get; set; }
    }
    ```

    #### JSON (string)
    ```json
    {
        "id": 123,
        "name": "Cuca Beludo",
        "items": [
            "Tia Romba"
            "Elma Maria Pinto",
            "Paula Vadão",
            "Mecânica Simas Turbo"
        ]
    }
    ```

    #### Controller Action
    ```cs
    // GET api/controller/get?sample={"id":123,"name":"Cuca Beludo","items":["Tia Romba","Elma Maria Pinto","Paula Vadão","Mecânica Simas Turbo"]}    


    // Este código ... 
    public IActionResult Get([FromJson] Sample sample)
    { 
        return Ok(sample);
    }

    // ...é equivalente a esse (com features a mais)
    public IActionResult Get([FromQuery] string sample)
    { 
        var obj = JsonSerializer.Deserialize<Sample>(sample);

        return Ok(obj);
    }
    ```
    </details>