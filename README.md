![](https://repobeats.axiom.co/api/embed/f5cead7dc8f370c3893a9f4a5edfd68b154ec43d.svg)

# LivrEtec
Bom dia, boa tarde ou boa noite. Esse projeto é um gerenciador do acervo da biblioteca da escola ETEC Aristóteles Ferreira

## Ferramentas
- **.NET Core**: Framework de todo Back-End
- **MySQL**: Banco de Dados
- **Entity Framwork**: ORM para conectar a aplicação ao banco
- **xUnit**: Para Executar Testes Unitarios da Biblioteca
- **Figma**: Para desenvolver o Design

## Como Executar
Atualmente só esta construída a biblioteca de classes, que outras partes do projeto futuramente irão usar. Então por enquanto é possivel apenas executar os testes.

### Executando Testes
Para executar os testes é preferível que esteja instalado o MySQL, porém, caso não esteja instalado o aplicativo executará usando um banco de dados in-memory.

#### CLI
Primeiramente verifique se está instalado o .NET 6.0 e o CLI dele, e então execute 
```bash
dotnet test
```
#### Visual Studio 
Abra o `./src/LivrEtec.sln` no Visual Studio, e então no menu Exibir (View em Inglês) abra o Gerenciador de Testes e aperte em executar.  

## Design 
O design está disponivel em figma.com/community/file/1176031299741420547
