# Ailos - Conta Corrente

Teste Prático

<br />

## Microserviço - Conta Corrente
Microserviço de Conta Corrente é usado para efetuar as seguintes operações:

<img width="1548" height="718" alt="image" src="https://github.com/user-attachments/assets/03621798-08e7-4a05-9853-52de62ac71bb" />

## Visão Geral do Projeto
Conforme pode ser verificado abaixo, a API foi desenvolvida utilizando Clean Code e boas práticas. Para mais detalhe, verificar a implementação.

<img width="465" height="817" alt="image" src="https://github.com/user-attachments/assets/07836f74-1220-4eaf-b2c3-995c57866cd1" />

<br />

## Pré-Requisitos

Antes de executar este projeto, os seguintes itens deverão estar instalados no computador:

* Docker instalado no computador onde será executado
* Para fins de praticidade, recomendo utilizar o Visual Studio, 2022 por exemplo, para a execução do projeto

<br />

Passo a passo - Execução:

* Baixe o projeto para seu computador
* Certifique-se de que não tem nenhum outro container em execução na sua máquina. Caso contrário, poderá gerar conflito de portas e a aplicação não funcionará
* Para fins de praticidade, recomendo utilizar o Visual Studio para a execução
* Efetue Clean e em seguida rebuild do Projeto, antes de executar:
  <img width="469" height="422" alt="image" src="https://github.com/user-attachments/assets/d5f81955-aa8e-4d58-84aa-60612d3883f2" />

* Defina o arquivo docker-compose como Startup Projetc:
* <img width="601" height="607" alt="image" src="https://github.com/user-attachments/assets/eb83b513-3942-4f00-9c9b-e757d1f47394" />

* Clique no botão:
  <img width="933" height="80" alt="image" src="https://github.com/user-attachments/assets/207c07af-e272-45b0-84b3-3260c2c4675f" />

* Após alguns seguundos, você deverá ver uma tela como esta:
  
<br />

## Testando a API

1 - o primeiro passo é cadastrar uma Conta Corrente; para assim ter um login e poder se autenticar na API;

2 - Vá até o endpoint "/ContaCorrente/CadastrarConta" e entre com os dados. Os valores abaixo são meramente para testes. O resultado deverá ser conforme abaixo:
<img width="1460" height="1065" alt="image" src="https://github.com/user-attachments/assets/92c2d924-b9a4-4e28-8908-b96e043f11bf" />

No banco de dados:
<img width="1272" height="234" alt="image" src="https://github.com/user-attachments/assets/ffb362ee-85a8-43cf-a32e-a76f7096f8ef" />


3 - Vá até o endpoint "/ContaCorrente/Logar" e informe os dados de acesso, conforme cadastrados no passo anterior. Você deverá receber o token JWT:
<img width="1590" height="923" alt="image" src="https://github.com/user-attachments/assets/4c4bc847-ef46-410a-a6f9-71bef0109fd8" />

4 - Vá até "Authorize" e se autentique na API, informando o token recebido no passo anterior:
<img width="1469" height="340" alt="image" src="https://github.com/user-attachments/assets/b0a5f415-e4a3-4b3e-a037-c0f68ae71037" />

5 - Guarde o token em algum editor de textos, pois ele será usado para se autenticar também na outra API (Transferência)

6 - Vá até o endpoint "/ContaCorrente/MovimentacaoContaCorrente" e crie um movimento de CRÉDITO, semelhante ao descrito:
<img width="1450" height="943" alt="image" src="https://github.com/user-attachments/assets/524260ae-222f-4b49-8c07-76167b3b699e" />

No banco de dados:
<img width="1198" height="213" alt="image" src="https://github.com/user-attachments/assets/b6b50c3b-a5f2-4f96-be37-08b863ed4bee" />

7 - Vá até o endpoint "/ContaCorrente/SaldoContaCorrente" e crie um consulte o saldo. O resultado será algo conforme abaixo:
<img width="1438" height="888" alt="image" src="https://github.com/user-attachments/assets/249ec2d7-15f4-4cd5-b41f-282ad6515a9b" />

8 - Vá até o endpoint "/ContaCorrente/MovimentacaoContaCorrente" e crie um movimento de DÉBITO, semelhante ao descrito:
<img width="1465" height="939" alt="image" src="https://github.com/user-attachments/assets/9a61035e-1b0d-4a7a-9a74-58a25e56271b" />

No banco de dados:
<img width="1037" height="188" alt="image" src="https://github.com/user-attachments/assets/c87e7d2a-3ed4-40c8-9501-74b4667df344" />

7 - Vá até o endpoint "/ContaCorrente/SaldoContaCorrente" e crie um consulte o saldo. O resultado será  conforme abaixo:
<img width="1482" height="882" alt="image" src="https://github.com/user-attachments/assets/dbdf9166-472b-414e-add7-c26e68feb36c" />






