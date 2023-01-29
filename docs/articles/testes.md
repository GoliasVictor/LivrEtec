# Testes
No LivrEtec são feitos os testes unitários de cada um dos serviços usando xUnit 

> Atualmente é apenas coberto os cenários positivos de testes, por causa que cobrir os negativos demandaria muito tempo    

Todos os serviços do projeto devem possuir suas respectivas classes de testes, e de preferencia que teste usando a interface do serviço, para que seja possível testar diferentes implementações dos serviços, usando as mesmas classes de teste, porque apesar das diferentes implementações das classes elas devem no final se comportar da mesma forma.


## Utilitários
Existem também os utilitários que servem para principalmente para compartilhar comportamentos que são repetidos em vários testes como a criação do base de dados (ou seus dublês). 
 