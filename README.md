# Make Up Store - API

API REST desenvolvida para a matéria Web III o curso Web Full Stack da ADA.  

Finalidade da api:

- Trata-se de uma base de dados de produtos cosméticos que simula os produtos oferecidos por uma loja aos clientes.
- A base de dados foi criada com base em alguns dados da api pública: [Makeup-api](https://makeup-api.herokuapp.com/)<br><br>  


## 🌐 Endpoints disponíveis

* **POST**     
  - <h3 style="color: #40b4e5">http:// localhost:5000/login</h3> 
  - <h3 style="color: #40b4e5">http:// localhost:5000/Produc</h3>    
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/query</h3>   

* **GET**           
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product</h3>  
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/{id}</h3><br>    

  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/brand</h3>  
    <em>Busca os produtos por marca.</em>  

    <em>Exemplo de algumas marcas disponíveis: benefit, clinique, covergirl, dior, l'oreal, maybelline, milani, nyx, revlon.</em> 

  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/allBrands</h3>   
    <em>Busca todas as marcas disponíveis e a quantidade de produtos em cada uma.</em><br><br>   
  
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/type</h3> 
    <em>Busca um produto por seu tipo e pela nota mínima desejada de avaliação.  

    Exemplo de alguns tipos de produtos disponíveis: blush, bronzer, eyebrow, eyeshadow, foundation, lipstick, mascara, nail polish.</em> 

* **PUT**     
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/{id}</h3>    

* **DELETE**    
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/{id}</h3>   
    
* **PATCH**     
  - <h3 style="color: #40b4e5">http:// localhost:5000/Product/{id}</h3>   

Objeto do método patch:

```
[  
  {  
    "path": "string",     
    "op": "string",   
    "value": "string"  
  }   
]   

```
Onde:  

- "path" é o atributo que se deseja alterar: name, brand, price, etc.
- "op" é a operação que se deseja realizar: "replace" para alterar o atributo, ou "remove" para excluir o valor do atributo. 
- "value" é o valor para o atributo. 

Exemplo de objeto para o método patch:  

```
[  
  {  
    "path": "price",   
    "op": "replace",  
    "value": "5.99"  
  }  
]  
```  
<br><br> 


## ⚙ Configurações

No arquivo <span style="color: #ffe69c"><em>appsettings.Development.json</em></span> deve-se configurar as seguintes variáveis:  

```
  "TokenConfiguration": {  
    "Secret": "secret do token",   
  },  
  "autenticacao": {  
    "login": "usuario",  
    "senha": "senha_secreta"   
  }  
```

