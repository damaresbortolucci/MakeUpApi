## Make Up Store - API

API REST desenvolvida para a matéria Web III o curso Web Full Stack da ADA.


Finalidade da api:

- Trata-se de uma base de dados de produtos cosméticos que simula os produtos oferecidos por uma loja aos clientes.
- A base de dados foi criada com base em alguns dados da api pública: [Makeup-api](https://makeup-api.herokuapp.com/)



### 🌐 Endpoints disponíveis

- (POST)      
http://localhost:5000/login  
http://localhost:5000/Product/query        
http://localhost:5000/Product  

- (GET)       
http://localhost:5000/Product           
http://localhost:5000/Product/{id}

- (PUT)       
http://localhost:5000/Product/{id}
    
- (PATCH)     
http://localhost:5000/Product/{id}

Objeto para o método patch:

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
    "value": "5.99'"  
  }  
]  
```

- (DELETE)    
http://localhost:5000/Product/{id}




### ⚙ Configurações

- No arquivo appsettings.Development.json deve-se configurar as seguintes variáveis:  

```
  "TokenConfiguration": {  
    "Secret": "secret do token",   
  },  
  "autenticacao": {  
    "login": "usuario",  
    "senha": "senha_secreta"   
  }  
```

