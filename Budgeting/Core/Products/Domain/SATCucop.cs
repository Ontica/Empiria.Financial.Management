/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Products                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Information Holder                      *
*  Type     : SATCucop                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Producto o servicio con partidas presupuestales del gasto corriente,                           *
*             de acuerdo al catálogo CUCoP del SAT México.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Products {

  /// <summary>Producto o servicio con partidas presupuestales del gasto corriente,
  /// de acuerdo al catálogo CUCoP del SAT México.</summary>
  public class SATCucop : GeneralObject {

    #region Constructors and parsers

    static public SATCucop Parse(int id) => ParseId<SATCucop>(id);

    static public SATCucop Parse(string uid) => ParseKey<SATCucop>(uid);

    static public FixedList<SATCucop> GetList() {
      return BaseObject.GetList<SATCucop>()
                       .ToFixedList();
    }

    static public SATCucop Empty => ParseEmpty<SATCucop>();

    #endregion Constructors and parsers

    #region Properties

    public string Clave {
      get {
        return ExtendedDataField.Get<string>("clave");
      }
      private set {
        ExtendedDataField.Set("clave", value);
      }
    }


    public string Partida {
      get {
        return ExtendedDataField.Get<string>("partida");
      }
      private set {
        ExtendedDataField.Set("partida", value);
      }
    }


    public string NombrePartida {
      get {
        return ExtendedDataField.Get<string>("nombrePartida");
      }
      private set {
        ExtendedDataField.Set("nombrePartida", value);
      }
    }


    public string Concepto {
      get {
        return ExtendedDataField.Get<string>("concepto");
      }
      private set {
        ExtendedDataField.Set("concepto", value);
      }
    }


    public string NombreConcepto {
      get {
        return ExtendedDataField.Get<string>("nombreConcepto");
      }
      private set {
        ExtendedDataField.Set("nombreConcepto", value);
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Clave, Name, Partida, NombrePartida, Concepto, NombreConcepto);
      }
    }

    #endregion Properties

  } // class SATCucop

} // namespace Empiria.Budgeting.Products
