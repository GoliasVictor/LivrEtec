# Telas

As telas estão disponíveis em https://www.figma.com/community/file/1176031299741420547
Este artigo se dedica a explicar o que deve haver em cada uma das tela, a descrição de cada uma delas está organizada em organizadas em,
 - Funcionalidade da tela
 - Ações possíveis
 - Estados possíveis.

## Tela Navegação
A navegação
 
## Tela Entrar
Nesta tela o usuário ira inserir seu login e senha, e caso ambos estejam certos ele terá acesso as outras telas.

E os estados da tela possíveis são:

## Tela Novo Livro
Nesta telo o usuário poderá [registrar](xref:gib.funcionalidades#gerenciar-livros) os dados do livro (disponível em [Modelos de Dados](xref:modelos-de-dados#livro)).

E os estados da tela possíveis são:
- Erro nome duplicado
- Erro valor obrigatório nulo
- Valor invalido de ISBN

## Tela Buscar Livros
Nesta tela o usuário poderá [buscar livros](xref:gib.funcionalidades#gerenciar-livros), com base no **nome do autor**, **nome do livro** e as **tags** que do livro, sendo o filtro por tag exclusivo. 

- Nenhuma tag definida 
## Tela Gerenciar Livro
Nesta tela o usuário visualizara as informações do livro e de quantos foram emprestado, e poderá:
- [Excluir o Livro](xref:gib.funcionalidades#gerenciar-livros)
- [Redireccionar para Empréstimo do livro](xref:gib.funcionalidades#Fazer-Empr%C3%A9stimo)
- [Redireccionar para Editar Livro](xref:gib.funcionalidades#gerenciar-livros)

Estados possíveis:
- Nenhum Exemplar Disponível
- Nenhum Livro no acervo
- Indisponível para fazer emprestimo

## Tela Editar Livro
Nesta tela o usuário poderá [editar](xref:gib.funcionalidades#gerenciar-livros)  os dados do livro (disponível em [Modelos de Dados](xref:modelos-de-dados#livro))
Estados possíveis:
- Erro nome já usado
- Erro nome vazio
- Modificando capa
- Adicionando tag
- Removendo tag



## Tela Fazer Empréstimo
Nesta tela é onde o [empréstimo é feito](xref:gib.funcionalidades#fazer-empréstimo), será possível adicionar uma pessoa ao empréstimo, definir o livro que será emprestado e sua data de devolução vem uma data calculada por padrão. E nesta tela devera ser mostrado um histórico resumido de todos, destacando pessoas que não devolveram sem justificativa .

Estados possíveis:
- Buscando livro
- Buscando usuario
- Erro Livro indisponível
- Aviso Histórico pessoa com empréstimo não devolvido
- Pessoa já com empréstimo  


## Tela Visualizar Todos Empréstimos
Nesta tela o usuário o usuário poderá [ver informações básicas dos empréstimos](xref:gib.funcionalidades#visualizar-empr%C3%A9stimos) dos empréstimos, e podendo filtrá-los por 
- Data empréstimo
- Data fechamento
- Livro 
- Pessoa 
- Estado(Aberto/Fechado)

Os estados possíveis de cada um dos empréstimos são
<dl>
  <dt>Em aguardo</dt>
  <dd><li>Fim do prazo</li></dd>
  <dt>Atrasado</dt> 
  <dd><li>Quanto tempo atrasado</li></dd>
  <dt>Fechado</dt>
  <dd>
    <li>data fechamento</li>
    <li>Houve Perda e se foi perdoada</li>   
  </dd>
</dl>

## Tela Gerenciar Empréstimo
Nesta tela mostrará todos os dados do empréstimo (disponíveis em [Modelos de Dados](xref:modelos-de-dados#empr%C3%A9stimo)
Ele poderá editar
- Data de devolução esperada
- Comentário

Nesta tela o usuário também poderá [fechar o empréstimo](xref:gib.funcionalidades#fechar-empr%C3%A9stimo) de duas formas
<dl>
  <dt>Livro devolvido</dt>
  <dd>O funcionário apenas confirma a devolução do livro e o sistema registra</dd>
  <dt>livro perdido </dt>
  <dd>Neste caso a pessoa afirma que não pode ter perdido o livro, porem informa uma justificativa valida, como ter sido roubada, então é registrado junto a justificativa que o livro foi perdido, ou não caso não haja justificativa ou simplismente a pessoa nunca mais apareça, é apenas registrado o fechamento sem devolução</dd>
</dl>
em todos os casos o funcionário pode adicionar um comentário sobre o fechamento do empréstimo.

Estados possíveis:
- Livro perdido
  - Justificado / Não Justificado
  - Justificativa perdoada


## Tela Configurações
Nas configurações sera onde o usuario ira gerenciar detalhes não muitos usuais mas que precisam ser acessados, sendo dividido em sub telas sendo elas

### Tela Gerenciar Tags
É onde o usuario ira fazer o [CRUD das tags](xref:gib.funcionalidades#gerenciar-tags-livros).

### Tela Gerenciar Autores
É onde o usuario ira fazer o [CRUD dos autores](xref:gib.funcionalidades#gerenciar-autores-livros).

### Tela Gerenciar Usuarios 
Nesta tela o usuario poderar fazer o [CRUD dos usuarios](xref:gib.funcionalidades#gerenciar-usuários). 
### Tela Gerenciar Cargos
É onde o usuário ira poder fazer os [CRUD dos cargos](xref:gib.funcionalidades#gerenciar-cargos), alem de definir quais permissões cada cargo possui. E o sistema deverá deixar marcado que para um cargo possuir certa permissão, ele deve possuir também outras permissões, por exemplo, para ter permissão de fechar um empréstimo é necessário a opção de visualiza-lo e edita-lo.

### Tela Exportação e Importação Livro
A ser documentada nesta tela, o usuário poderá fazer duas ações, ou importar livros, onde o usuário insere o arquivo no sistema, o sistema o avalia as informações dos livros e mostra quais livros serão importados, e então o usuário confirma ou cancela a importação. Ou pode também exportar os dados, escolhendo o tipo de arquivo deseja exportar, e então escolhendo o local onda deverá ser salvo e então salvo.