---
uid: gib.funcionalidades
---
# Funcionalidades

Todos os gerenciar, precisam ter as operações básicas de criação, visualização, edição e exclusão de que se referem. 

## Gerenciar livros
Uma das principais funções do aplicativo é o gerenciamento de quais livros estão na biblioteca.
 
### Importar e Exportar Dados Livros
  Sera necessário haver a possibilidade de importar dados vindos de uma planilha Excel. E tambem Devera ser possível que seja exportado dados de todos os livros para algum tipo de arquivo excel, json ou csv.
### Gerenciar Autores Livros
  Necessário apenas funcionalidades básicas de criação, visualização, edição e exclusão.

### Gerenciar Tags Livros 
  Necessário apenas funcionalidades básicas de criação, visualização, edição e exclusão.
## Gerenciar Estante  

## Gerenciar Emprestimos
  Outra funcionalidade principal sera o gerenciamento dos empréstimo, 
### Fazer Empréstimo 
  O aluno ou professor vai até a biblioteca, escolhe o livro que deseja, solicita para o funcionário da biblioteca e ele então registra o livro, a pessoa e quando deve ser devolvido, que por padrão será alguns dias apos a data de registro, e esse tempo está definido nas configurações. E o sistema registra isto.
 No momento do registro deve ser possível ver o histórico de empréstimo daquela pessoa que está pegando emprestado o livro.
### Prorrogar empréstimo
Deverá ser possível o usuário poderá prorrogar a entrega de um ou vários livros de algum dia em especifico. 

### Fechar empréstimo
 Há dois casos de fechamento de empréstimo

- **Livro Devolvido:**
 Onde o aluno vai até a biblioteca e devolve o livro que tinha pego, estando atrasado ou não, então o funcionário registra a devolução no sistema.
 - **Livro Perdido:**
 Onde é concluído que por algum motivo ouve uma perda do livro por exemplo a pessoa não devolver depois de meses, então o funcionário registra que se perdeu o livro, junto da justificativa desta perda.
### Visualizar Empréstimos
  Devera ser possível visualizar os empréstimos, tendo alguns filtros sendo eles:
- Por pessoa 
- Por Fechado/Abertos/Todos
- Em atraso

## Gerenciar Pessoas
Ainda precisa ser melhor analisado

## Gerenciar Usuários
Os usuários são as pessoas que interagem com o sistema.
### Entrar no sistema
Cada usuário possui um login e senha para entrar no sistema, para então poder ter outras interações.

## Gerenciar cargos 
Cada usuário possui um cargo, que define quais são as ações que ele pode fazer dentro do sistema, e cada cargo é basicamente a lista de permissões, e no sistema deverá ser possível o usuário criar, editar as permissões do cargo e excluir cargos (menos o cargo de administrador). E as permissões podem ser interdependentes por exemplo, para um usuário poder excluir um livro ele deve poder 

