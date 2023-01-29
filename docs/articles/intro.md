# Visão geral
O repositório atualmente possui 5 projetos, sendo 3 deles bibliotecas compartilhadas, 1 de API e por ultimo um de teste
- **LivrEtec:**  aqui é onde esta os modelos de dados usados em toda as aplicações, as exceções comuns e as interfaces dos serviços. È o Núcleo de todos projetos.
- **LivrEtec.GIB:** aqui é onde esta os arquivos protos que geram a base para o cliente e para o servidor `gRPC` em que o `GIB` se comunica, e as conversões dos modelos `gRPC` gerados para os modelos usados no ambiente Livretec. 
- **LivrEtec.Servidor:** Aqui é onde está a implementação dos serviços no lado do servidor, que sera usada por todos (porém ha um problema relacionado a isso https://github.com/GoliasVictor/LivrEtec/issues/60)
- **LivrEtec.GIB.Servidor:** Este é o projeto do servidor do `GIB`, que recebe as solicitações via `gRPC`, e os executa em relação aos respectivos serviços solicitados. 
- **LivrEtec.Testes:** Aqui estão todos os testes do projeto.

# Escolha de arquitetura
A arquitetura do projeto é baseada em SOA(arquitetura orientada a serviços), onde o projeto **LivrEtec**, é onde esta as interfaces dos serviços usadas em todas as aplicações. A ideia por trás da escolha dessa arquitetura, é a possibilidade de abstrair os serviços de uma forma em que as aplicações apenas chamem eles sem saber suas implementações, fazendo com que seja possível migrar modificar a forma com que um serviço funciona internamente sem precisar modificar suas chamadas, por exemplo sera possível testar os serviços tanto de forma remota quanto local, porque no `GIB`, o cliente `gRPC` terá a mesma interface que o serviço funcionando localmente, assim, não precisando implementar duas vezes os testes. 


