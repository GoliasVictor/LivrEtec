![](https://repobeats.axiom.co/api/embed/f5cead7dc8f370c3893a9f4a5edfd68b154ec43d.svg)

# LivrEtec
Bom dia, boa tarde ou boa noite. Esse projeto é um gerenciador do acervo da biblioteca da escola ETEC Aristóteles Ferreira
## Projetos
Neste repositorio vai existir tres projetos:
- **GIB** (Gerenciador interno da biblioteca) que será usado pelos funcionarios da biblioteca gerenciarem os livros e emprestimos
- **APB** (Acervo publico da biblioteca) que será usado pelos proprios alunos para visualizar o acervo de livros contidos na biblioteca. 
- **Api publica** para professores proporem aos alunos a usarem para testar chamadas HTTP e tambem popular sites feito por alunos.
## Ferramentas
- **Docker**: Para isolar os ambientes de teste;
- **.NET Core**: Plataforma de todo Back-End
- **MySQL**: Banco de Dados
- **Entity Framwork**: ORM para conectar a aplicação ao banco
- **gRPC**: Como protocolo de comunicação entre o servidor GIB e o cliente 
- **Blazor Hybrid + MAUI**: Interface do cliente GIB
- **xUnit**: Para Executar Testes Unitarios da Biblioteca
- **Figma**: Para desenvolver o Design

## Como Executar
Atualmente só esta construída a biblioteca de classes, que outras partes do projeto futuramente irão usar. Então por enquanto é possivel apenas executar os testes.

### Executando Testes
Para executar os testes é preferível que esteja instalado o MySQL, porém, caso não esteja instalado o aplicativo executará usando um banco de dados in-memory. 

Caso deseje testar com o MySql, verifique de ou editar o arquivo `.src/LivrEtec.Testes/appsettings.json` ou configurar a variavel de ambiente `APP_SETTINGS_JSON_PATH` para sua respectiva configuração.

#### CLI
Primeiramente verifique se está instalado o .NET 6.0 e o CLI dele, e então execute na pasta `./src/` o seguinte comando
```bash
dotnet test
```
#### Visual Studio 
Abra o `./src/LivrEtec.sln` no Visual Studio, e então no menu Exibir (View em Inglês) abra o Gerenciador de Testes e aperte em executar.  
#### Docker
Caso deseje usar o docker, apenas vaia até a pasta `./src/` e execute o levante os containers com o docker compose com o seguinte comando
```bash
docker compose up --build
``` 
O `--build` é para garantir que o container seja executado com o codigo mais recente.

Depois de executar os testes o container do banco de dados ira continuar executando, caso queria que saia automaticamente apos os testes adiciona `--exit-code-from app` ao comando. 
## Design 
O design está disponivel em https://figma.com/community/file/1176031299741420547

## Documentação
Para mais informações sobre o projeto vaia na [wiki](https://github.com/GoliasVictor/LivrEtec/wiki) onde está documentado o projeto.