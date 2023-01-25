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
- **Figma**: Para prototipar o design

## Como Executar
Atualmente está completo em partes o API interna do GIB feita em gRPC, e também é possível executar o testes unitários do projeto.
### Executando API do GIB
É possível executar a API de duas formas, usando o ambiente da maquina, ou usando docker, caso tenha o docker instalado ou não consiga instalar os requisitos pra executar o projeto no ambiente da maquina, é recomendado a execução no docker, porém dependendo da internet pode demorar para instalação das imagens e dos pacotes.
#### Levantando em Ambiente local
Para executar no ambiente local há os seguintes requisitos 

- .NET 6.0
- MySql

Também é necessário antes da execução que você configure o arquivo `./src/LivrEtec.GIB.Servidor/appsettings.json`, recomendo que copie o arquivo [`appsettings.modelo.json`](./src/LivrEtec.GIB.Servidor/appsettings.modelo.json) e preencha a propriedade AuthKey primeiramente com uma string qualquer que servira como chave de autenticação, de preferencia que seja uma sequencia aleatória de caracteres, e também preencha a string de conexão com o seu servidor MySql. Caso não entenda como preencher use como exemplo o [appsettings usado no docker](src/Docker/GIB.Servidor/appsettings.json)

Após configurar o projeto, você pode tanto abrir o projeto no visual studio e selecionar o projeto de iniciação como LivrEtec.GIB.Servidor, como também executar no terminal, abrindo o terminal na pasta `./src/LivrEtec.GIB.Servidor` e executando o seguinte comando:
```bash
dotnet run
```
Pode acontecer erro caso não tenha configurado o appsettings corretamente, como string de conexão invalida, ou escolher uma porta http invalida. E em caso de qualquer erro se sinta confortável para [postar um issue](https://github.com/GoliasVictor/LivrEtec/issues/new)  

#### Levantando usando Docker

Para levantar o servidor usando docker é simples, basta ter instalado e rodando o docker em sua maquina e executar o seguinte comando  na pasta `./src`
```bash
docker compose --profile backend up --build
```
> Como dito anteriormente,como é necessário baixar as imagens e pacotes NuGet pode demorar minutos para baixar dependendo da sua internet. 

#### Chamando 

Após executar o projeto, ele informara as portas que ele esta ouvindo, é recomendado que use a porta http, por causa que não está configurado certificados https ainda

Para fazer as chamadas a api escolha o cliente gRPC a sua escolha, algumas alternativas são o [Insomnia](https://docs.insomnia.rest/insomnia/grpc) ou o [Postman](https://learning.postman.com/docs/sending-requests/grpc/grpc-request-interface/), o arquivo proto da api está em [./src/LivrEtec.GIB/Protos/acervo.proto](./src/LivrEtec.GIB/Protos/acervo.proto).


### Executando os Testes
Para executar os testes é preferível que tenha instalado o MySQL, porém, caso não esteja instalado, o aplicativo utilizará um banco de dados In-Memory. 

Caso deseje testar com o MySQL, editar o arquivo `.src/LivrEtec.Testes/appsettings.json` ou configurar a variável de ambiente `APP_SETTINGS_JSON_PATH` para sua respectiva configuração.

#### CLI
Primeiramente, verifique se está instalado o .NET 6.0.0
```bash
dotnet --list-sdks
```

E então execute na pasta `./src/` o seguinte comando:
```bash
dotnet test
```

#### Visual Studio 
Abra o `./src/LivrEtec.sln` no Visual Studio, e então no menu Exibir (View em Inglês) abra o Gerenciador de Testes e clique em Executar.

#### Docker
Caso deseje usar o docker, apenas vá até a pasta `./src/` e execute os containers com o docker compose com o seguinte comando:
```bash
docker compose up --build
``` 
> O `--build` é para garantir que o container seja executado com o código mais recente.

> Depois de executar os testes, o container do banco de dados irá continuar executando. Caso queria que saia automaticamente após os testes adicione `--exit-code-from app` ao comando.

## Design 
O design está disponível em https://figma.com/community/file/1176031299741420547

## Como colaborar 
Toda ajuda é bem vinda, caso queira colaborar. Recomendamos que primeiro vá na (Wiki)[./wiki/Arquitetura-dos-Projetos] e leia um pouco para ter a visão geral do projeto, e então procure nas (Issues)[./issues] qualquer problema que lhe interesse consertar. Se encontrar algum crie um fork do projeto, faça suas modificações e então faça um pull request para o projeto.
Garanta que antes de fazer o pull request, tenha criado os testes para suas novas implementações, e de que esteja passando em todos os testes.

## Documentação
Para mais informações sobre o projeto, vá na [Wiki](https://github.com/GoliasVictor/LivrEtec/wiki) onde está a documentação do projeto.

Seja bem-vindo :)
