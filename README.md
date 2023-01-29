![](https://repobeats.axiom.co/api/embed/f5cead7dc8f370c3893a9f4a5edfd68b154ec43d.svg)

# LivrEtec
Bom dia, boa tarde ou boa noite. Esse projeto é um gerenciador do acervo da biblioteca da escola Etec Aristóteles Ferreira

## Projetos
Neste repositório vai existir três projetos:
- **GIB** (Gerenciador Interno da Biblioteca) usado pelos funcionários da biblioteca para gerenciar os livros e empréstimos.
- **APB** (Acervo Público da Biblioteca) usado pelos alunos para visualizar o acervo de livros contidos da biblioteca. 
- **API pública** para professores proporem aos alunos a usarem, para testar chamadas HTTP e também popular sites feito por alunos.

## Ferramentas
- **Docker**: Para isolar os ambientes de teste
- **.NET Core**: Plataforma de todo o Back-End
- **MySQL**: Banco de Dados
- **Entity Framework**: ORM para conectar a aplicação ao banco de dados
- **gRPC**: Como protocolo de comunicação entre o servidor GIB e o cliente 
- **Blazor Hybrid + MAUI**: Interface do cliente GIB
- **xUnit**: Para Executar Testes Unitários das bibliotecas
- **DocFx**: Para Criação da documentação
- **Figma**: Para prototipar o design

## Como Executar
Atualmente está parcialmente completa a API interna do GIB feita em gRPC. É possível executar os testes unitários do projeto.
### Executando API do GIB
É possível executar a API de duas formas: usando o ambiente de máquina ou usando Docker. É preferível o uso do Docker caso já tenha instalado ou não consiga instalar os requisitos para a execução no ambiente de máquina.

> Pode ser que, devido à velocidade de banda, demore para instalar as imagens e pacotes.

#### Executando em ambiente local
Para executar em ambiente local, há os seguintes requisitos:

- **.NET 6.0.0**
- **MySQL**

É necessário que antes da execução configurar o arquivo `./src/LivrEtec.GIB.Servidor/appsettings.json` 

> Recomendo que copie o arquivo [appsettings.modelo.json](./src/LivrEtec.GIB.Servidor/appsettings.modelo.json), preenchendo a propriedade _AuthKey_ com uma string qualquer que sirva como chave de autenticação e a string de conexão de acordo com seu servidor MySQL. Use como exemplo o arquivo [appsettings usado no docker](src/Docker/GIB.Servidor/appsettings.json).

Após configurar o projeto, você abrir o projeto no Visual Studio e escolher o **LivrEtec.GIB.Servidor** como projeto de iniciação, ou, executar num emulador de terminal no diretório `./src/LivrEtec.GIB.Servidor` com o seguinte comando:
```bash
dotnet run
```
Pode ocorrer algum erro caso não tenha configurado o appsettings corretamente, como string de conexão ou porta inválida. Em caso de qualquer erro, sinta-se confortável para [postar uma issue](https://github.com/GoliasVictor/LivrEtec/issues/new)  

#### Executando usando Docker

Para executar o servidor usando Docker é simples, basta executar o seguinte comando na pasta `./src`
```bash
docker compose --profile backend up --build
```
> Como dito anteriormente,como é necessário baixar as imagens e pacotes NuGet pode demorar minutos para baixar dependendo da sua banda de internet. 

#### Chamando 

Após executar o projeto, ele informará nas portas que está ouvindo. Como ainda não estão configurados certificados SSL, é recomendado usar padrões HTTP.

Para fazer as chamadas à API, escolha o cliente gRPC a sua escolha.

Algumas alternativas são o [Insomnia](https://docs.insomnia.rest/insomnia/grpc) ou o [Postman](https://learning.postman.com/docs/sending-requests/grpc/grpc-request-interface/).

O arquivo proto da api está em [./src/LivrEtec.GIB/Protos/acervo.proto](./src/LivrEtec.GIB/Protos/acervo.proto).

Após isso, é necessário fazer login na API através de um request para `login` em `GerenciamentoSessao`. Para fazer o login são necessários o nome de login e o hash da senha, porém, na primeira vez iniciando o servidor, ele irá criar no banco de dados um usuário com login `admin:senha` e `id` 1.

> O hash da senha precisa ser gerado da seguinte maneira: `md5(senha + id)`. 

Então o request deve ficar da seguinte maneira: 
```json 
{
	"IdUsuario": 1,
	"HashSenha": "92f20dafc5e5ac1c66820903c492cc04"
}
```
 
E então ele retornará o token JWT, que deve ser adicionado a header da seguinte maneira:
`Authorization: Bearer <JWT Token>` 

Pronto, agora você terá acesso ao resto da API como administrador.

### Executando os Testes

Assim como a API do GIB é possível testar no ambiente local e no Docker, porém, é possível fazer alguns testes sem um servidor MySQL instalado.

#### Ambiente Local
Os testes são divididos em dois:
* Os testes locais que testam cada um dos serviços no próprio processo do teste usando SqlLite
* Testes remotos que são os testes de integração.

Para executar apenas os testes locais, basta ter instalado o .NET 6.0.0 e na pasta `./src/` executar o seguinte comando:
```bash
dotnet test LivrEtec.sln --filter Category=local
```

Caso esteja usando Visual Studio abra o gerenciador de teste e execute a categoria de testes `local`


Para executar também testes remotos é necessário primeiro [executar o servidor do GIB](#executando-api-do-gib). Após ter executado, é necessário que crie e configure o arquivo `./src/LivrEtec.Testes/appsettings.json`.
> Copie o arquivo [appsettings.modelo.json](./src/LivrEtec.Testes/appsettings.modelo.json), e preencha com as mesma informações que preencheu o appsettings do servidor do GIB, além de adicionar o link do servidor, caso o link apareça neste formato: `http://[::]:21312` significa o mesmo que `http://localhost:21312` 

E após ter configurado o servidor e os testes para executar o projeto basta executar o seguinte comando no diretório `./src/`
```bash
dotnet test LivrEtec.sln
```
No Visual Studio basta abrir o Gerenciador de Testes e clicar em executar.


#### Docker
Caso deseje usar o docker, apenas vá até a pasta `./src/` e execute os containers com o seguinte comando:
```bash
docker compose --profile teste up --build
``` 
> O `--build` é para garantir que o container seja executado com o código mais recente.

> Depois de executar os testes, o container do banco de dados irá continuar executando. Caso queira que saia automaticamente após os testes, adicione `--exit-code-from app` como flag no comando.

## Design 
O design está disponível em https://figma.com/community/file/1176031299741420547

## Como colaborar 
Toda ajuda é bem vinda, caso queira colaborar. Recomendamos que primeiro vá na (Wiki)[./wiki/Arquitetura-dos-Projetos] e leia um pouco para ter a visão geral do projeto, e então procure nas (Issues)[./issues] qualquer problema que lhe interesse resolver. Se encontrar algum crie um fork do projeto, faça suas modificações e então faça um pull request para o projeto.
Garanta que antes de fazer o pull request, tenha criado os testes para suas novas implementações, e de que esteja passando em todos os testes.

## Documentação
Para mais informações sobre o projeto, vá na [Wiki](https://github.com/GoliasVictor/LivrEtec/wiki) onde está a documentação do projeto.

Seja bem-vindo :)
