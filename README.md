﻿# Empiria Financial Management

Este producto de software está siendo desarrollado a la medida para el Banco Nacional de Obras y Servicios Públicos, S.N.C (BANOBRAS).

[BANOBRAS](https://www.gob.mx/banobras) es una institución de banca de desarrollo mexicana cuya labor
es financiar obras para la creación de servicios públicos. Por el tamaño de su cartera de crédito directo,
es el cuarto Banco más grande del sistema bancario mexicano y el primero de la Banca de Desarrollo de nuestro país.

Este repositorio contiene los módulos del *backend* del **Sistema de administración financiera**.

Todos los módulos están escritos en C# 7.0 y utilizan .NET Framework versión 4.8.  
Los módulos pueden ser compilados utilizando [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/).

El acceso a los servicios que ofrece el *backend* se realiza mediante llamadas a servicios web de tipo RESTful,
mismos que están basados en ASP .NET.

Al igual que otros productos Empiria, este *backend* se apoya en [Empiria Framework](https://github.com/Ontica/Empiria.Core),
[Empiria Central](https://github.com/Ontica/Empiria.Central), y también en algunos módulos de
[Empiria Extensions](https://github.com/Ontica/Empiria.Extensions).

## Contenido

El *backend* del **Sistema de administración financiera** se conforma de los siguientes módulos:

1.  **Core**  
    Tipos, clases y servicios de propósito general que conforman el núcleo del *backend* del sistema de administración financiera.  

2.  **Billing**  
    Sistema para la administración de la facturación.  

3.  **Budgeting**  
    Sistema de planeación, programación y control presupuestal.  

4.  **Cashflow**  
    Sistema para la administración del flujo de efectivo.  

5.  **Contracts**  
    Sistema para la administración de contratos con proveedores.  

6.  **Payments**  
    Sistema para la administración de pagos a proveedores, reembolsos y administración de fondos fijos.  

7. **Web API**  
    Cada uno de los sistemas o subistemas provee su propia capa de servicios web HTTP/Json.  

8. **Tests**  
    Cada uno de los sistemas o subistemas provee sus propios módulos de pruebas unitarias y de integración.  


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
