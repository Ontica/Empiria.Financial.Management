/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : SATProducto                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Producto o servicio de acuerdo al catálogo del SAT México para facturación electrónica.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing.SATCatalogues {

  /// <summary>Producto o servicio de acuerdo al catálogo del SAT México para facturación electrónica.</summary>
  public class SATProducto : GeneralObject {

    #region Constructors and parsers

    static public SATProducto Parse(int id) => ParseId<SATProducto>(id);

    static public SATProducto Parse(string uid) => ParseKey<SATProducto>(uid);

    static public FixedList<SATProducto> GetList() {
      return BaseObject.GetList<SATProducto>()
                       .ToFixedList();
    }

    static public SATProducto Empty => ParseEmpty<SATProducto>();

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


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Name, Clave);
      }
    }

    #endregion Properties

  } // class SATProducto

} // namespace Empiria.Billing.SATCatalogues
