﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:2.0.50727.8009
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Estoque
{

    private EstoqueMensagem mensagemField;

    private SignatureType signatureField;

    private string versaoField;

    /// <remarks/>
    public EstoqueMensagem Mensagem
    {
        get
        {
            return this.mensagemField;
        }
        set
        {
            this.mensagemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public SignatureType Signature
    {
        get
        {
            return this.signatureField;
        }
        set
        {
            this.signatureField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Versao
    {
        get
        {
            return this.versaoField;
        }
        set
        {
            this.versaoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class EstoqueMensagem
{

    private EstoqueMensagemEstabelecimento estabelecimentoField;

    private EstoqueMensagemPafEcf pafEcfField;

    private EstoqueMensagemDadosEstoque dadosEstoqueField;

    /// <remarks/>
    public EstoqueMensagemEstabelecimento Estabelecimento
    {
        get
        {
            return this.estabelecimentoField;
        }
        set
        {
            this.estabelecimentoField = value;
        }
    }

    /// <remarks/>
    public EstoqueMensagemPafEcf PafEcf
    {
        get
        {
            return this.pafEcfField;
        }
        set
        {
            this.pafEcfField = value;
        }
    }

    /// <remarks/>
    public EstoqueMensagemDadosEstoque DadosEstoque
    {
        get
        {
            return this.dadosEstoqueField;
        }
        set
        {
            this.dadosEstoqueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class EstoqueMensagemEstabelecimento
{

    private string ieField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string Ie
    {
        get
        {
            return this.ieField;
        }
        set
        {
            this.ieField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class EstoqueMensagemPafEcf
{

    private string numeroCredenciamentoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string NumeroCredenciamento
    {
        get
        {
            return this.numeroCredenciamentoField;
        }
        set
        {
            this.numeroCredenciamentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class EstoqueMensagemDadosEstoque
{

    private System.DateTime dataReferenciaField;

    private EstoqueMensagemDadosEstoqueProduto[] produtosField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime DataReferencia
    {
        get
        {
            return this.dataReferenciaField;
        }
        set
        {
            this.dataReferenciaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Produto", IsNullable = false)]
    public EstoqueMensagemDadosEstoqueProduto[] Produtos
    {
        get
        {
            return this.produtosField;
        }
        set
        {
            this.produtosField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class EstoqueMensagemDadosEstoqueProduto
{

    private string descricaoField;

    private string codigoGTINField;

    private string codigoCESTField;

    private string codigoNCMSHField;

    private string codigoProprioField;

    private string quantidadeField;

    private string quantidadeTotalAquisicaoField;

    private string unidadeField;

    private string valorUnitarioField;

    private string valorTotalAquisicaoField;

    private string valorTotalICMSDebitoFornecedorField;

    private string valorBaseCalculoICMSSTField;

    private string valorTotalICMSSTField;

    private EstoqueMensagemDadosEstoqueProdutoSituacaoTributaria situacaoTributariaField;

    private string aliquotaField;

    private bool isArredondadoField;

    private EstoqueMensagemDadosEstoqueProdutoIppt ipptField;

    private EstoqueMensagemDadosEstoqueProdutoSituacaoEstoque situacaoEstoqueField;

    /// <remarks/>
    public string Descricao
    {
        get
        {
            return this.descricaoField;
        }
        set
        {
            this.descricaoField = value;
        }
    }

    /// <remarks/>
    public string CodigoGTIN
    {
        get
        {
            return this.codigoGTINField;
        }
        set
        {
            this.codigoGTINField = value;
        }
    }

    /// <remarks/>
    public string CodigoCEST
    {
        get
        {
            return this.codigoCESTField;
        }
        set
        {
            this.codigoCESTField = value;
        }
    }

    /// <remarks/>
    public string CodigoNCMSH
    {
        get
        {
            return this.codigoNCMSHField;
        }
        set
        {
            this.codigoNCMSHField = value;
        }
    }

    /// <remarks/>
    public string CodigoProprio
    {
        get
        {
            return this.codigoProprioField;
        }
        set
        {
            this.codigoProprioField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string Quantidade
    {
        get
        {
            return this.quantidadeField;
        }
        set
        {
            this.quantidadeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string QuantidadeTotalAquisicao
    {
        get
        {
            return this.quantidadeTotalAquisicaoField;
        }
        set
        {
            this.quantidadeTotalAquisicaoField = value;
        }
    }

    /// <remarks/>
    public string Unidade
    {
        get
        {
            return this.unidadeField;
        }
        set
        {
            this.unidadeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string ValorUnitario
    {
        get
        {
            return this.valorUnitarioField;
        }
        set
        {
            this.valorUnitarioField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string ValorTotalAquisicao
    {
        get
        {
            return this.valorTotalAquisicaoField;
        }
        set
        {
            this.valorTotalAquisicaoField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string ValorTotalICMSDebitoFornecedor
    {
        get
        {
            return this.valorTotalICMSDebitoFornecedorField;
        }
        set
        {
            this.valorTotalICMSDebitoFornecedorField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string ValorBaseCalculoICMSST
    {
        get
        {
            return this.valorBaseCalculoICMSSTField;
        }
        set
        {
            this.valorBaseCalculoICMSSTField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string ValorTotalICMSST
    {
        get
        {
            return this.valorTotalICMSSTField;
        }
        set
        {
            this.valorTotalICMSSTField = value;
        }
    }

    /// <remarks/>
    public EstoqueMensagemDadosEstoqueProdutoSituacaoTributaria SituacaoTributaria
    {
        get
        {
            return this.situacaoTributariaField;
        }
        set
        {
            this.situacaoTributariaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "token")]
    public string Aliquota
    {
        get
        {
            return this.aliquotaField;
        }
        set
        {
            this.aliquotaField = value;
        }
    }

    /// <remarks/>
    public bool IsArredondado
    {
        get
        {
            return this.isArredondadoField;
        }
        set
        {
            this.isArredondadoField = value;
        }
    }

    /// <remarks/>
    public EstoqueMensagemDadosEstoqueProdutoIppt Ippt
    {
        get
        {
            return this.ipptField;
        }
        set
        {
            this.ipptField = value;
        }
    }

    /// <remarks/>
    public EstoqueMensagemDadosEstoqueProdutoSituacaoEstoque SituacaoEstoque
    {
        get
        {
            return this.situacaoEstoqueField;
        }
        set
        {
            this.situacaoEstoqueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum EstoqueMensagemDadosEstoqueProdutoSituacaoTributaria
{

    /// <remarks/>
    Isento,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Nao tributado")]
    Naotributado,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Substituicao tributaria")]
    Substituicaotributaria,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Tributado pelo ICMS")]
    TributadopeloICMS,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Tributado pelo ISSQN")]
    TributadopeloISSQN,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum EstoqueMensagemDadosEstoqueProdutoIppt
{

    /// <remarks/>
    Proprio,

    /// <remarks/>
    Terceiros,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public enum EstoqueMensagemDadosEstoqueProdutoSituacaoEstoque
{

    /// <remarks/>
    Positivo,

    /// <remarks/>
    Negativo,
}