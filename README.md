Este projeto é uma API desenvolvida em .NET 8 (C#) com abordagem Code First utilizando Entity Framework Core, e está completamente dockerizado para facilitar a execução e o setup do ambiente de desenvolvimento.

O ambiente é composto por três serviços, definidos via docker-compose:
- API .NET 8
- SQL Server (como base de dados)
- SMTP4DEV (servidor SMTP fake para ambiente de desenvolvimento)

Para rodar todo o ambiente, é necessário ter o Docker Desktop instalado.
Basta executar a seguinte linha a partir de um CMD Prompt na raiz do projeto (onde se encontra o docker-compose.yml):

	docker-compose up --build
	
Isso fará o build e a execução dos três serviços automaticamente.
Após esse passo, a aplicação estará disponível na URL:

	http://localhost:5078/api
	
O servidor SMTP4DEV poderá ser acessado no Browser via:

	http://localhost:5000
	
Este projeto utiliza EF Core Migrations com a estratégia Code First, ou seja, o banco de dados é criado a partir do modelo de entidades do código.

Em geral, ao rodar o projeto via Docker, as migrations são aplicadas automaticamente no container da API.
Caso haja problemas ao aplicar as migrations via Docker:

É possível gerar manualmente o script SQL correspondente à migration e aplicá-lo diretamente no banco utilizando uma IDE como o SQL Server Management Studio ou o Azure Data Studio.
Para isso, utilize o seguinte comando:

	dotnet ef migrations script --project Escola.Data --startup-project Escola.API --output D:\scriptBaseDeDadosEscola.sql
	
O arquivo "Escola - Acessos a API Backend.postman_collection.json" é uma exportação do conjunto de acessos no Postman.
