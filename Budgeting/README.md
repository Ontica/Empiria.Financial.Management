# Empiria Budgeting System

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/bee7df368e8e4e7e9d1da0c8aeca0bed)](https://app.codacy.com/gh/Ontica/Empiria.Budgeting/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
&nbsp; &nbsp;
[![Maintainability](https://api.codeclimate.com/v1/badges/8a247a73100dca989c0c/maintainability)](https://codeclimate.com/github/Ontica/Empiria.Budgeting/maintainability)

Este producto de software está siendo desarrollado a la medida para el Banco Nacional de Obras y Servicios Públicos, S.N.C (BANOBRAS).

[BANOBRAS](https://www.gob.mx/banobras) es una institución de banca de desarrollo mexicana cuya labor
es financiar obras para la creación de servicios públicos. Por el tamaño de su cartera de crédito directo,
es el cuarto Banco más grande del sistema bancario mexicano y el primero de la Banca de Desarrollo de nuestro país.

Este repositorio contiene los módulos del *backend* del **Sistema de control presupuestal**.

Todos los módulos están escritos en C# 7.0 y utilizan .NET Framework versión 4.8.  
Los módulos pueden ser compilados utilizando Visual Studio 2022 Community Edition.

El acceso a los servicios que ofrece el *backend* se realiza mediante llamadas a servicios web de tipo RESTful,
mismos que están basados en ASP .NET.

Al igual que otros productos Empiria, este *backend* se apoya en [Empiria Framework](https://github.com/Ontica/Empiria.Core),
y también en algunos módulos de [Empiria Extensions](https://github.com/Ontica/Empiria.Extensions).


## Contenido

El *backend* del **Sistema de control presupuestal** se conforma de los siguientes módulos:

1.  **Core**  
    Tipos, clases y servicios de propósito general que conforman el núcleo del *backend*.  

2.  **Explorer**  
    Servicios para obtener información de presupuestos agrupada y consolidada de diferentes formas. 

3.  **Processes**  
    Administra los procesos presupuestales. 

4.  **Transactions**  
    Permite generar y almacenar transacciones presupuestales. Cada transacción tiene una o más  
    entradas las cuales están asociadas a las cuentas que conforman los presupuestos. 

5. **Web API**  
    Capa de servicios web HTTP/Json para interactuar con todos los módulos que conforman el *backend* del sistema.  


## Licencia

Este producto y sus partes se distribuyen mediante una licencia GNU AFFERO
GENERAL PUBLIC LICENSE, para uso exclusivo de BANOBRAS y de su personal, y
también para su uso por cualquier otro organismo en México perteneciente a
la Administración Pública Federal.

Para cualquier otro uso (con excepción a lo estipulado en los Términos de
Servicio de GitHub), es indispensable obtener con nuestra organización una
licencia distinta a esta.

Lo anterior restringe la distribución, copia, modificación, almacenamiento,
instalación, compilación o cualquier otro uso del producto o de sus partes,
a terceros, empresas privadas o a su personal, sean o no proveedores de
servicios de las entidades públicas mencionadas.

El desarrollo, evolución y mantenimiento de este producto está siendo pagado
en su totalidad con recursos públicos, y está protegido por las leyes nacionales
e internacionales de derechos de autor.

## Copyright

Copyright © 2024-2025. La Vía Óntica SC, Ontica LLC y autores.
Todos los derechos reservados.
