# Criação banco de dados
Há o problema de que quando o GIB é iniciado pela primeira vez, não é possível fazer nenhuma interação com a API interna por não existir nenhum usuário (é necessário estar logado). No cenário de produção não é algo problemático porque é possível apenas executar um pequeno script que cria o usuário administrador, sem grandes problemas, porém, no caso do ambiente de desenvolvimento ou um desenvolvedor abrindo o projeto pela primeira vez tem tal incômodo de ter que criar ná mão o banco de dados variás vezes, por isso, foi adicionado um pequeno script logo antes da API Interna do GIB ser executado que cria o usuário administrador com a senha "senha" caso o banco exista, apenas para motivos de teste. Futuramente sera substituído por algo decente.