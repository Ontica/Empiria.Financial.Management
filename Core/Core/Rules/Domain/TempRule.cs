/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : TempRule                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Temporal financial rule (PYC oldies).                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Rules.Data;

namespace Empiria.Financial.Rules {

  public class TempRule {

    static private readonly FixedList<TempRule> _rules;

    static TempRule() {
      _rules = TempRulesData.GetTempRules();
    }


    static public FixedList<TempRule> GetList() {
      return _rules;
    }


    #region Properties

    [DataField("ID_RULE")]
    public int Id {
      get; private set;
    }


    [DataField("CONCEPTO")]
    public int Concepto {
      get; private set;
    }


    [DataField("DESCRIPCION")]
    public string Descripcion {
      get; private set;
    }


    [DataField("NATURALEZA")]
    public int Naturaleza {
      get; private set;
    }


    [DataField("CUENTA_CONTABLE")]
    public string CuentaContable {
      get; private set;
    }


    [DataField("AUXILIAR")]
    public string Auxiliar {
      get; private set;
    }

    #endregion Properties

  } // class TempRule

} // namespace Empiria.Financial
