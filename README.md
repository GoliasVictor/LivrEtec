# LivrEtec
Bom dia, Boa tarde, Boa noite, Gerenciador do acervo da biblioteca da escola ETEC Aristóteles Ferreira
## Ferramentas  
- **.Net Core**: Como Framework de todo Backend
- **MySql**: Como provedor do Banco de Dados
- **Entity Framwork**: ORM para conectar aplicação ao banco
- **xUnit**: Para Executar Testes Unitarios da Biblioteca 
- **Figma**: Para desenvolver o Design 
## Como Executar
O projeto atualmente só esta construida a biblioteca de classes que outras partes do projeto futuramente irão usar, então por enquanto é possivel apenas execurtar os testes

### Executando Testes

Para executar os testes é preferivel que esteja instalado o MySql, porem caso não esteja instalado o aplicativo executara usando um Banco de Dados em memoria
#### CLI
Primeiramente verifique de estar instalado o .Net 6.0 e o CLI dele, e então execute 
```bash
dotnet test
```
#### Visual Studio 
Abra o `./src/LivrEtec.sln` no Visual Studio, e então no menu Exibir(View em Ingles) abra o Gerenciador de Destes e aperte em executar.  

## Design 
O design do projeto está disponivel em https://www.figma.com/community/file/1176031299741420547

