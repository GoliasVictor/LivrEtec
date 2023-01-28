# SRC
Olá, este é o diretório do código fonte do projeto, aqui um pequeno guia do que encontra aqui

- [Docker](./src/Docker): Neste diretório estão os arquivos de configuração do docker, como o dockerfile e os appsettings.json
- [LivrEtec](./LivrEtec/): Aqui é onde se encontra o núcleo de todo o projeto, onde as peças fundamentais compartilhadas por todo o projeto estão, como os modelos de dados.
- [LivrEtec.Servidor](./LivrEtec.Servidor): Onde a maior parte da logica do sistema está, aqui é onde fica os repositórios e serviços que irão manipular diretamente o banco de dados, e serve de base para a parte do servidor do resto do sistema.
- [LivrEtec.GIB](./LivrEtec.GIB): Atualmente é onde está definido o arquivo Proto da api gRPC do GIB, e onde é definido a conversão dos modelos do gRPC para do LivrEtec e vice-versa. 
- [LivrEtec.GIB.Servidor](./LivrEtec.GIB.Servidor/): É onde é implementada o servidor da api definida em [LivrEtec.GIB](./LivrEtec.GIB), sua principal responsabilidade é basicamente traduzir os requests feitos na API para chamadas dos serviços já implementados em  [LivrEtec.Servidor](./LivrEtec.Servidor/)
- [LivrEtec.Testes](./LivrEtec.Testes/): Aqui é onde é garantido que as coisas funcionam como devem, é onde está os testes unitários e de integração.