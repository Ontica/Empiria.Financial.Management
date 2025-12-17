/* Empiria Central  ******************************************************************************************
*                                                                                                            *
*  Module   : Financial                                  Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Integration interface                   *
*  Type     : IPayableEntity                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines a payable entity. Payable entities are purchase orders,                                *
*             contract supply orders, invoices, paychecks, etc.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Financial {

  /// <summary>Defines a payable entity. Payable entities are purchase orders,
  /// contract supply orders, invoices, paychecks, etc.</summary>
  public interface IPayableEntity : IIdentifiable, INamedEntity {

    string EntityNo {
      get;
    }


    string Description {
      get;
    }

    INamedEntity OrganizationalUnit {
      get;
    }

    INamedEntity PayTo {
      get;
    }

    INamedEntity Currency {
      get;
    }

    decimal Total {
      get;
    }

    INamedEntity Budget {
      get;
    }


    INamedEntity Project {
      get;
    }

    FixedList<IPayableEntityItem> Items {
      get;
    }


    ObjectTypeInfo GetEmpiriaType();

  }  // interface IPayableEntity


  /// <summary>Defines a payable entity item.</summary>
  public interface IPayableEntityItem {

    int Id {
      get;
    }

    string UID {
      get;
    }

    decimal Quantity {
      get;
    }

    INamedEntity Unit {
      get;
    }

    decimal UnitPrice {
      get;
    }

    INamedEntity Currency {
      get;
    }

    INamedEntity Product {
      get;
    }

    string Description {
      get;
    }

    INamedEntity BudgetAccount {
      get;
    }


    decimal Subtotal {
      get;
    }

  }  // interface IPayableEntityItem

}  // namespace Empiria.Financial
