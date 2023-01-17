![](https://repobeats.axiom.co/api/embed/f5cead7dc8f370c3893a9f4a5edfd68b154ec43d.svg)

# LivrEtec
Bom dia, boa tarde ou boa noite. Esse projeto é um gerenciador do acervo da biblioteca da escola ETec Aristóteles Ferreira

## Projetos
Neste repositório vai existir três projetos:
- **GIB** (Gerenciador Interno da Biblioteca) usado pelos funcionários da biblioteca para gerenciar os livros e empréstimos.
- **APB** (Acervo Público da Biblioteca) usado pelos alunos para visualizar o acervo de livros contidos da biblioteca. 
- **API pública** para professores proporem aos alunos a usarem, para testar chamadas HTTP e tambem popular sites feito por alunos.

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
Atualmente só está construída a biblioteca de classes, que futuramente outras partes do projeto irão usar. Então por enquanto só é possivel executar os testes.

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
O design está disponivel em https://figma.com/community/file/1176031299741420547

## Como colaborar 
Toda ajuda é bem vinda, caso queira colaborar. Recomendamos que primeiro vá na (Wiki)[./wiki/Arquitetura-dos-Projetos] e leia um pouco para ter a visão geral do projeto, e então procure nas (Issues)[./issues] qualquer problema que lhe interesse consertar. Se encontrar algum crie um fork do projeto, faça suas modificações e então faça um pull request para o projeto.
Garanta que antes de fazer o pull request, tenha criado os testes para suas novas implementações, e de que esteja passando em todos os testes.

## Documentação
Para mais informações sobre o projeto, vá na [Wiki](https://github.com/GoliasVictor/LivrEtec/wiki) onde está a documentação do projeto.

Seja bem-vindo :)
