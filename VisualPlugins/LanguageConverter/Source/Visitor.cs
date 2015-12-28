using PascalABCCompiler.SemanticTree;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using Converter;
using System.Text;
using System.IO;

namespace VisualPascalABCPlugins
{
    public class SemanticTreeVisitor : ISemanticVisitor
    {
        private System.Windows.Forms.TreeNodeCollection nodes;        

        private ISemanticNodeConverter ISemanticNodeConverter;

        private ICommonNamespaceNode mainNamespace;
        private string mainFunction = "";
        private int NumNestedStatement = 0;
        // e,hfnm yfabu
        public List<string> nmspaceFiles = new List<string>();

        // убрать отсюда
        public void SaveTextInFile(string fileName, string text, bool append)
        {
            StreamWriter sw = new StreamWriter(fileName, append, System.Text.Encoding.GetEncoding(1251));
            sw.Write(text);
            sw.Close();
        }

        public string GetConvertedText()
        {
            return ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
        }

        public SemanticTreeVisitor(ISemanticNodeConverter _semanticNodeConverter)
        {
            ISemanticNodeConverter = _semanticNodeConverter;
        }                     

        // для обычного node - потомка ISemanticNode
        public void prepare_node(ISemanticNode subnode, string node_name)
        {
            
        }
        
        public void visit(ISemanticNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IDefinitionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ITypeNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IBasicTypeNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonTypeNode value)
        {             
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeType("type", value));
        }

        public void visit(IStatementNode value)
        {
            // abstract
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack("");
        }

        public void visit(IExpressionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IFunctionCallNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IBasicFunctionCallNode value)
        {
            for (int i = value.real_parameters.Length-1; i >= 0 ;i--)
                value.real_parameters[i].visit(this);  
                                         
            switch (value.basic_function.basic_function_type)
            {
                case basic_function_type.none: return;

                case basic_function_type.iassign: 
                case basic_function_type.bassign: 
                case basic_function_type.lassign: 
                case basic_function_type.fassign: 
                case basic_function_type.dassign: 
                case basic_function_type.uiassign:
                case basic_function_type.usassign:
                case basic_function_type.ulassign:
                case basic_function_type.sassign: 
                case basic_function_type.sbassign:
                case basic_function_type.boolassign:
                case basic_function_type.charassign:
                case basic_function_type.objassign:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeAssign("binop_assign", value));
                    break;

                ///////////                

                case basic_function_type.iadd:
                case basic_function_type.badd:
                case basic_function_type.sadd:
                case basic_function_type.ladd:
                case basic_function_type.fadd:
                case basic_function_type.sbadd:
                case basic_function_type.usadd:
                case basic_function_type.uladd:
                case basic_function_type.dadd: 
                case basic_function_type.uiadd:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeAdd("binop_add", value));
                    break;

                    // minus
                case basic_function_type.isub:
                case basic_function_type.bsub:
                case basic_function_type.ssub:
                case basic_function_type.lsub:
                case basic_function_type.fsub:
                //case basic_function_type.uisub:
                case basic_function_type.sbsub:
                case basic_function_type.ussub:
                case basic_function_type.ulsub:
                case basic_function_type.dsub: 
                case basic_function_type.uisub:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeSubtract("binop_subtract", value));
                    break;

                case basic_function_type.imul:
                case basic_function_type.bmul:
                case basic_function_type.smul:
                case basic_function_type.lmul:
                case basic_function_type.fmul:
                //case basic_function_type.uimul:
                case basic_function_type.sbmul:
                case basic_function_type.usmul:
                case basic_function_type.ulmul:
                case basic_function_type.dmul: 
                case basic_function_type.uimul:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeMult("binop_mult", value));
                    break;

                case basic_function_type.idiv:
                case basic_function_type.bdiv:
                case basic_function_type.sdiv:
                case basic_function_type.ldiv:
                case basic_function_type.fdiv:
                //case basic_function_type.uidiv:
                case basic_function_type.sbdiv:
                case basic_function_type.usdiv:
                case basic_function_type.uldiv:
                case basic_function_type.ddiv: 
                case basic_function_type.uidiv:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeDiv("binop_div", value));
                    break;

                case basic_function_type.imod:
                case basic_function_type.bmod:
                case basic_function_type.smod:
                //case basic_function_type.uimod:
                case basic_function_type.sbmod:
                case basic_function_type.usmod:
                case basic_function_type.ulmod:
                case basic_function_type.lmod: 
                case basic_function_type.uimod:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeMod("binop_mod", value));
                    break;

                    // increment
                case basic_function_type.isinc:
                case basic_function_type.bsinc:
                case basic_function_type.sbsinc:
                case basic_function_type.ssinc:
                case basic_function_type.ussinc:
                case basic_function_type.uisinc:
                case basic_function_type.ulsinc:
                case basic_function_type.lsinc: 
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeInc("binop_inc", value));
                    break;

                    // decrement
                case basic_function_type.isdec:
                case basic_function_type.bsdec:
                case basic_function_type.sbsdec:
                case basic_function_type.ssdec:
                case basic_function_type.ussdec:
                case basic_function_type.uisdec:
                case basic_function_type.ulsdec: 
                case basic_function_type.lsdec:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeDec("binop_dec", value));
                    break;

                    // not
                case basic_function_type.inot:
                case basic_function_type.bnot:
                case basic_function_type.snot:
                case basic_function_type.uinot:
                case basic_function_type.sbnot:
                case basic_function_type.usnot:
                case basic_function_type.ulnot:
                case basic_function_type.lnot:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeBinNot("binop_not", value));
                    break;
                case basic_function_type.boolsdec: 
                case basic_function_type.boolsinc:
                    throw new System.Exception("UNknown operation!!!");
                case basic_function_type.boolnot: //il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUnNot("unop_not", value));
                    break;
                
                    //  <<
                case basic_function_type.ishl: 
                case basic_function_type.bshl: 
                case basic_function_type.sshl:
                case basic_function_type.lshl:
                case basic_function_type.uishl:
                case basic_function_type.sbshl:
                case basic_function_type.usshl:
                case basic_function_type.ulshl:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeShl("binop_shl", value));
                    break;
                    // >>
                case basic_function_type.ishr:    
                case basic_function_type.bshr: 
                case basic_function_type.sshr: 
                case basic_function_type.lshr: 
                case basic_function_type.uishr:
                case basic_function_type.sbshr:
                case basic_function_type.usshr:
                case basic_function_type.ulshr:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeShr("binop_shr", value));
                    break;

                    // ==
                case basic_function_type.ieq:
                case basic_function_type.seq:
                case basic_function_type.beq:
                case basic_function_type.leq:
                case basic_function_type.uieq:
                case basic_function_type.sbeq:
                case basic_function_type.useq:
                case basic_function_type.uleq:
                case basic_function_type.booleq:
                case basic_function_type.feq: 
                case basic_function_type.deq: 
                case basic_function_type.chareq:
                case basic_function_type.objeq:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeEq("binop_eq", value));
                    break;
                    // !=
                case basic_function_type.inoteq:
                case basic_function_type.snoteq:
                case basic_function_type.bnoteq:
                case basic_function_type.lnoteq:
                case basic_function_type.uinoteq:
                case basic_function_type.sbnoteq:
                case basic_function_type.usnoteq:
                case basic_function_type.ulnoteq:
                case basic_function_type.boolnoteq:
                case basic_function_type.fnoteq:
                case basic_function_type.dnoteq:
                case basic_function_type.charnoteq:
                case basic_function_type.objnoteq:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeNotEq("binop_noteq", value));
                    break;
                    // >
                case basic_function_type.igr:
                case basic_function_type.sgr:
                case basic_function_type.bgr:
                case basic_function_type.lgr:
                case basic_function_type.uigr:
                case basic_function_type.sbgr:
                case basic_function_type.usgr:
                case basic_function_type.ulgr:
                case basic_function_type.boolgr:
                case basic_function_type.fgr:
                case basic_function_type.dgr:
                case basic_function_type.chargr:
                case basic_function_type.enumgr:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeGr("binop_gr", value));
                    break;
                    // >=
                case basic_function_type.igreq:
                case basic_function_type.sgreq:
                case basic_function_type.bgreq:
                case basic_function_type.lgreq:
                case basic_function_type.uigreq:
                case basic_function_type.sbgreq:
                case basic_function_type.usgreq:
                case basic_function_type.ulgreq:
                case basic_function_type.boolgreq:
                case basic_function_type.fgreq:
                case basic_function_type.dgreq:
                case basic_function_type.chargreq:
                case basic_function_type.enumgreq:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeGrEq("binop_greq", value));
                    break;
                    // <
                case basic_function_type.ism:
                case basic_function_type.ssm:
                case basic_function_type.bsm:
                case basic_function_type.lsm:
                case basic_function_type.uism:
                case basic_function_type.sbsm:
                case basic_function_type.ussm:
                case basic_function_type.ulsm:
                case basic_function_type.boolsm:
                case basic_function_type.fsm:
                case basic_function_type.dsm:
                case basic_function_type.charsm:
                case basic_function_type.enumsm:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeSm("binop_sm", value));
                    break;

                    // <=
                case basic_function_type.ismeq:                    
                case basic_function_type.ssmeq:                                                    
                case basic_function_type.bsmeq:                                
                case basic_function_type.lsmeq:                                
                case basic_function_type.uismeq:                               
                case basic_function_type.sbsmeq:                               
                case basic_function_type.ussmeq:                               
                case basic_function_type.ulsmeq:                               
                case basic_function_type.boolsmeq:                             
                case basic_function_type.fsmeq:                                
                case basic_function_type.dsmeq:                                
                case basic_function_type.charsmeq:
                case basic_function_type.enumsmeq:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeSmEq("binop_smeq", value));
                    break;
                    
                case basic_function_type.band:
                case basic_function_type.iand:
                case basic_function_type.land:
                case basic_function_type.sand:
                case basic_function_type.uiand:
                case basic_function_type.sband:
                case basic_function_type.usand:
                case basic_function_type.uland:
                case basic_function_type.booland:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeAnd("binop_and", value));
                    break;

                case basic_function_type.bor:
                case basic_function_type.ior:
                case basic_function_type.lor:
                case basic_function_type.sor:
                case basic_function_type.uior:
                case basic_function_type.sbor:
                case basic_function_type.usor:
                case basic_function_type.ulor:
                case basic_function_type.boolor:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeOr("binop_or", value));
                    break;

                case basic_function_type.bxor:
                case basic_function_type.ixor:
                case basic_function_type.lxor:
                case basic_function_type.sxor:
                case basic_function_type.uixor:
                case basic_function_type.sbxor:
                case basic_function_type.usxor:
                case basic_function_type.ulxor:
                case basic_function_type.boolxor:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeXor("binop_xor", value));
                    break;
                //case basic_function_type.chareq: il.Emit(OpCodes.Ceq); break;
                //case basic_function_type.charnoteq: il.Emit(OpCodes.Ceq); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); break;                                                                                    
                    
                case basic_function_type.iunmin:
                case basic_function_type.bunmin:
                case basic_function_type.sunmin:
                case basic_function_type.funmin:
                case basic_function_type.dunmin:
                //case basic_function_type.uiunmin:
                case basic_function_type.sbunmin:
                case basic_function_type.usunmin:
                //case basic_function_type.ulunmin:
                case basic_function_type.lunmin:
                case basic_function_type.uiunmin:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUnMin("binop_unmin", value));
                    break;

                case basic_function_type.iinc:
                case basic_function_type.binc:
                case basic_function_type.sinc:
                case basic_function_type.linc:
                case basic_function_type.uiinc:
                case basic_function_type.sbinc:
                case basic_function_type.usinc:
                case basic_function_type.ulinc:
                case basic_function_type.boolinc:
                case basic_function_type.cinc:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUnInc("unop_inc", value));
                    break;

                case basic_function_type.idec:
                case basic_function_type.bdec:
                case basic_function_type.sdec:
                case basic_function_type.ldec:
                case basic_function_type.uidec:
                case basic_function_type.sbdec:
                case basic_function_type.usdec:
                case basic_function_type.uldec:
                case basic_function_type.booldec:
                case basic_function_type.cdec:
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUnDec("unop_dec", value));
                    break;

                    /*
                case basic_function_type.chartoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.chartous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.chartoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.chartol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.chartoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.chartof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.chartod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.chartob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.chartosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.chartos: il.Emit(OpCodes.Conv_I2); break;

                case basic_function_type.itod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.itol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.itof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.itob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.itosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.itos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.itous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.itoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.itoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.itochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.btos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.btous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.btoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.btoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.btol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.btoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.btof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.btod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.btosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.btochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.sbtos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.sbtoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.sbtol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.sbtof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.sbtod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.sbtob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.sbtous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.sbtoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.sbtoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.sbtochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.stoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.stol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.stof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.stod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.stob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.stosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.stous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.stoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.stoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.stochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ustoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ustoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ustol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ustoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ustof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ustod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ustob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ustosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ustos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ustochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.uitoi: il.Emit(OpCodes.Conv_I4); break;
                //case basic_function_type.ustoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.uitol: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.uitoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.uitof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.uitod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.uitob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.uitosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.uitos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.uitous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.uitochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ultof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ultod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ultob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ultosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ultos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ultous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ultoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ultoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ultol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ultochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ltof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.ltod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ltob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ltosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ltos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ltous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ltoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ltoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ltoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ltochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.ftod: il.Emit(OpCodes.Conv_R8); break;
                case basic_function_type.ftob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.ftosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.ftos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.ftous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.ftoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.ftoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.ftol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.ftoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.ftochar: il.Emit(OpCodes.Conv_U2); break;

                case basic_function_type.dtob: il.Emit(OpCodes.Conv_U1); break;
                case basic_function_type.dtosb: il.Emit(OpCodes.Conv_I1); break;
                case basic_function_type.dtos: il.Emit(OpCodes.Conv_I2); break;
                case basic_function_type.dtous: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.dtoi: il.Emit(OpCodes.Conv_I4); break;
                case basic_function_type.dtoui: il.Emit(OpCodes.Conv_U4); break;
                case basic_function_type.dtol: il.Emit(OpCodes.Conv_I8); break;
                case basic_function_type.dtoul: il.Emit(OpCodes.Conv_U8); break;
                case basic_function_type.dtof: il.Emit(OpCodes.Conv_R4); break;
                case basic_function_type.dtochar: il.Emit(OpCodes.Conv_U2); break;
                case basic_function_type.booltoi: il.Emit(OpCodes.Conv_I4); break;

                case basic_function_type.objtoobj:
                    {
                        NETGeneratorTools.PushCast(il, helper.GetTypeReference(fn.type).tp);
                        break;
                    }
                */
            }
        }

        public void visit(ICommonNamespaceFunctionCallNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length-1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            if (bodyBlock.ToString() == "")
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeFunctionCall("function_call", value));
        }

        public void visit(ICommonNestedInFunctionFunctionCallNode value)
        {
            //prepare_string_node(value.last_result_function_call.ToString(), "ICommonNestedInFunctionFunctionCallNode.last_result_function_call"); ?
            //throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonMethodCallNode value)
        {

        }

        public void visit(ICommonStaticMethodCallNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length - 1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());            
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeStaticMethodCall("comm_stat_method_call", value));
        }

        public void visit(ICompiledMethodCallNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length - 1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            if (bodyBlock.Length == 0)
                bodyBlock.Append("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());            
            value.obj.visit(this);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeCompMethodCall("comp_method_call", value));
        }

        public void visit(ICompiledStaticMethodCallNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length - 1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            if (bodyBlock.Length == 0)
                bodyBlock.Append("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());            
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeStaticMethodCall("comp_stat_method_call", value));
        }

        public void visit(IFunctionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IClassMemberNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledClassMemberNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonClassMemberNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledTypeNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IFunctionMemberNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }
        
        public void visit(INamespaceMemberNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IBasicFunctionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonFunctionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());            
        }

        public void visit(ICommonNamespaceFunctionNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            StringBuilder parameters = new StringBuilder("");
            // увеличиваем отступ тела блока перед входом в блок

            // тело функции
            if (value.var_definition_nodes.Length != 0)
                ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyIncrement();
            foreach (ILocalVariableNode nodeVariable in value.var_definition_nodes)
            {
                nodeVariable.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());                
                bodyBlock.Append(";");
                bodyBlock.Append(System.Environment.NewLine);
                //bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
            }
            // если блок не пуст - уменьшаем отступ
            if (bodyBlock.ToString() != "")
            {
                //ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
                ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyDecrement();
            }

            value.function_code.visit(this);
            // на стеке лежит тело функции
            //  
            string funcbody = ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
            bodyBlock.Append(funcbody);
            if (funcbody == "")
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack("%empty%");
            else
            {
                funcbody += System.Environment.NewLine;
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(funcbody);
            }
            
            foreach (IParameterNode nodeParameter in value.parameters)
            {
                nodeParameter.visit(this);
                parameters.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.parameters[value.parameters.Length-1] != nodeParameter)
                    parameters.Append(", ");
            }
            // кладем на стек тело функции 
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            // кладем на стек параметры ф-ции
            if (parameters.Length == 0)
                parameters.Append("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(parameters.ToString());                       

            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeFunction("function", value));
        }

        public void visit(ICommonNestedInFunctionFunctionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonMethodNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledMethodNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IIfNode value)
        {
            if (value.else_body != null)
                value.else_body.visit(this);
            value.then_body.visit(this);
            value.condition.visit(this);
            if (value.else_body == null)
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeIf("if", value));
            else
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeIfElse("if_else", value));                
        }

        public void visit(IWhileNode value)
        {
            value.body.visit(this);
            value.condition.visit(this);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeWhile("while", value));
        }

        public void visit(IRepeatNode value)
        {
            value.body.visit(this);
            value.condition.visit(this);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeRepeat("repeat", value));
        }

        public void visit(IForNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            StringBuilder parameters = new StringBuilder("");
            value.body.visit(this);
            if (value.initialization_statement != null)
            {
                value.initialization_statement.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
            }
            bodyBlock.Append(";");
            //bodyBlock.Append(" ");
            if (value.while_expr != null)
            {
                value.while_expr.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
            }
            bodyBlock.Append(";");
            //bodyBlock.Append(" ");
            if (value.increment_statement != null)
            {
                value.increment_statement.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
            }
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeFor("for", value));
        }

        public void visit(IStatementsListNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            NumNestedStatement++;
            // увеличиваем отступ тела блока перед входом в блок
            ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyIncrement();
            foreach (ILocalBlockVariableNode nodeVar in value.LocalVariables)
            {
                nodeVar.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                bodyBlock.Append(";");
                //if (value.LocalVariables[value.LocalVariables.Length - 1] != nodeVar)
                    bodyBlock.Append(System.Environment.NewLine);
            }

            foreach (IStatementNode nodeStatement in value.statements)
            {
                nodeStatement.visit(this);
                string snode = ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
     //           if (snode != "")
                {
                    if (snode != "")
                        bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                    bodyBlock.Append(snode);
                    if (!(nodeStatement is IStatementsListNode))
                        if (snode != "") 
                            bodyBlock.Append(";");
                    if (value.statements[value.statements.Length - 1] != nodeStatement)
                        bodyBlock.Append(System.Environment.NewLine);
                }
                //else
                    //ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack("%empty%");
            }

            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            // уменьшаем отступ тела блока перед выходом из блока
            ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyDecrement();
            if (NumNestedStatement > 1)
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeStatementsNested("statements_nested", value));
            else
            {
                string snode = ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
                if (snode == "")
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack("%empty%");                    
                else
                    ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(snode);
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeStatements("statements", value));
            }
            NumNestedStatement--;
        }

        public void visit(INamespaceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonNamespaceNode value)
        {
            //value.Location.document.file_name
            StringBuilder bodyBlock = new StringBuilder("");
            // увеличиваем отступ тела блока перед входом в блок
            ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyIncrement();
            //bodyBlock.Append(SemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);

            // если находимся в главном namespace программы, добваляем main в body
            if (mainNamespace == value)
            {
                bodyBlock.Append(System.Environment.NewLine);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(mainFunction);
                //SemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(mainFunctionNode);     
            }  
            foreach (ICommonNamespaceNode nodeNamespace in value.nested_namespaces)
            {
                nodeNamespace.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                //if (value.nested_namespaces[value.nested_namespaces.Length - 1] != nodeNamespace)
                    bodyBlock.Append(System.Environment.NewLine);                
            }   
            //
            foreach (ICommonTypeNode types in value.types)
            {
                types.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                //if (value.types[value.types.Length - 1] != types)
                    bodyBlock.Append(System.Environment.NewLine);
                
            }
            foreach (ICommonNamespaceVariableNode var in value.variables)
            {
                var.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                bodyBlock.Append(";");
                //if (value.variables[value.variables.Length - 1] != var)
                    bodyBlock.Append(System.Environment.NewLine);
            }            
            foreach (ICommonNamespaceFunctionNode functions in value.functions)
            {
                functions.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                //bodyBlock.Append(";");
                bodyBlock.Append(System.Environment.NewLine);
            }
            foreach (INamespaceConstantDefinitionNode constants in value.constants)
            {
                constants.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                 bodyBlock.Append(";");
                bodyBlock.Append(System.Environment.NewLine);
            }             

            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            // уменьшаем отступ тела блока перед выходом из блока            
            ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyDecrement();
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeNamespace("namespace", value));
            
        }

        public void visit(ICompiledNamespaceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IDllNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IProgramNode value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            string currNamespace = "";

            // обнуляем отступ тела блокаame
            ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyDecrement();                                                 

            if (value.main_function != null)
                value.main_function.visit(this);
            mainFunction = ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody + ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
                        
            // перед входом в блок ув. отступ
            //SemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBodyIncrement();
            mainNamespace = value.main_function.comprehensive_namespace;
            string outdir = System.IO.Path.GetDirectoryName(mainNamespace.Location.document.file_name);
            foreach (ICommonNamespaceNode nodeNamespace in value.namespaces)
            {
                nodeNamespace.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.TextFormatter.Indents.BlockBody);
                currNamespace = ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack();
                bodyBlock.Append(currNamespace);                                                               
                if (value.namespaces[value.namespaces.Length-1] != nodeNamespace)
                    bodyBlock.Append(System.Environment.NewLine);
                string name = nodeNamespace.namespace_name.Replace('$', 'S');
                if (name == "")
                    name = System.IO.Path.GetFileNameWithoutExtension(mainNamespace.Location.document.file_name);
                SaveTextInFile(outdir + "\\" +name+".cs", currNamespace, false);
                nmspaceFiles.Add(outdir + "\\" + name + ".cs");
            }
            //bodyBlock.Append(mainFunctionNode);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeProgram("program", value));
        }

        public void visit(IAddressedExpressionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ILocalVariableReferenceNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeLocVariableReference("locvariable_reference", value));
        }

        public void visit(INamespaceVariableReferenceNode value)
        {            
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeVariableReference("variable_reference", value));
        }

        public void visit(ICommonClassFieldReferenceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IStaticCommonClassFieldReferenceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledFieldReferenceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IStaticCompiledFieldReferenceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonParameterReferenceNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeParameterReference("parameter_reference", value));
        }


        public void visit(IConstantNode value)
        {
            // abstract
        }

        public void visit(IBoolConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeBoolConstant("bool_constant", value));
        }

        public void visit(IByteConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeByteConstant("byte_constant", value));
        }

        public void visit(IIntConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeIntConstant("int_constant", value));
        }

        public void visit(ISByteConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeSByteConstant("sbyte_constant", value));
        }

        public void visit(IShortConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeShortConstant("short_constant", value));
        }

        public void visit(IUShortConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUShortConstant("ushort_constant", value));
        }

        public void visit(IUIntConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeUIntConstant("uint_constant", value));
        }

        public void visit(IULongConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeULongConstant("ulong_constant", value));
        }

        public void visit(ILongConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeLongConstant("long_constant", value));
        }

        public void visit(IDoubleConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeDoubleConstant("double_constant", value));
        }

        public void visit(IFloatConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeFloatConstant("float_constant", value));
        }

        public void visit(ICharConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeCharConstant("char_constant", value));
        }

        public void visit(IStringConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeStringConstant("string_constant", value));
        }


        public void visit(IVAriableDefinitionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ILocalVariableNode value)
        {
            StringBuilder bodyBlock = new StringBuilder();
                     
            if (value.inital_value == null)
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeVariable("var", value));            
            else
            {
                // кладем на стек сгенерированное в новый текст значение переменной
                value.inital_value.visit(this);   
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeVariable("var_init", value));
            }
        }

        public void visit(ICommonNamespaceVariableNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeNamespaceVariable("namespace_var", value));
        }

        public void visit(ICommonClassFieldNode value)
        {

            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledClassFieldNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IParameterNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonParameterNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeParameter("parameter", value));
        }

        public void visit(IBasicParameterNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledParameterNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IConstantDefinitionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IClassConstantDefinitionNode value)
        {

            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledClassConstantDefinitionNode value)
        {
       
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(INamespaceConstantDefinitionNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeNamespaceConst("const", value));
        }

        public void visit(ICommonFunctionConstantDefinitionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IPropertyNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonPropertyNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IBasicPropertyNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledPropertyNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IThisNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IReturnNode value)
        {
            value.return_value.visit(this);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeReturn("return", value));
        }

        public void visit(ICommonConstructorCall value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length - 1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            if (bodyBlock.Length == 0)
                bodyBlock.Append("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeConstructorCall("constructor_call", value));            
        }

        public void visit(ICompiledConstructorCall value)
        {
            StringBuilder bodyBlock = new StringBuilder("");
            foreach (IExpressionNode nodeParameter in value.real_parameters)
            {
                nodeParameter.visit(this);
                bodyBlock.Append(ISemanticNodeConverter.SourceTextBuilder.GetNodeFromStack());
                if (value.real_parameters[value.real_parameters.Length - 1] != nodeParameter)
                    bodyBlock.Append(", ");
            }
            if (bodyBlock.Length == 0)
                bodyBlock.Append("%empty%");
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(bodyBlock.ToString());
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeConstructorCall("constructor_call", value));            
        }

        public void visit(IWhileBreakNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IRepeatBreakNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IForBreakNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IWhileContinueNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IRepeatContinueNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IForContinueNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledConstructorNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ISimpleArrayNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ISimpleArrayIndexingNode value)
        {
            value.index.visit(this);
            value.array.visit(this);
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeSimpleArrInd("array_indexing", value));
        }

        public void visit(IExternalStatementNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IRefTypeNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IGetAddrNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IDereferenceNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IThrowNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ISwitchNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICaseVariantNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICaseRangeNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(INullConstantNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeNullConst("const_null", value));
        }

        public void visit(IUnsizedArray value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IRuntimeManagedMethodBody value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IAsNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IIsNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ISizeOfOperator value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ITypeOfOperator value)
        {                      
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeTypeOf("type_of", value));
        }

        public void visit(IExitProcedure value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ITryBlockNode value)
        {

            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IExceptionFilterBlockNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IArrayConstantNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IStatementsExpressionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IQuestionColonExpressionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IRecordConstantNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ILabelNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ILabeledStatementNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IGotoStatementNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICompiledStaticMethodCallNodeAsConstant value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(ICommonNamespaceFunctionCallNodeAsConstant value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IEnumConstNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IForeachNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
            //prepare_node(value.Body, "IForeachNode.Body"); ?
            //prepare_node(value.InWhatExpr, "IForeachnode.InWhatExpr"); ?
            //prepare_node(value.VarIdent, "IForeachNode.VarIdent"); ?            
        }

        public void visit(ILockStatement value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
            //prepare_node(value.Body, "ILockStatement.Body"); ?
            //prepare_node(value.LockObject, "ILockStatement.LockObject"); ?          
        }

        public void visit(ILocalBlockVariableNode value)
        {
            StringBuilder bodyBlock = new StringBuilder();

            if (value.inital_value == null)
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeVariable("var", value));
            else
            {
                // кладем на стек сгенерированное в новый текст значение переменной
                value.inital_value.visit(this);
                ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeVariable("var_init", value));
            }
        }

        public void visit(ILocalBlockVariableReferenceNode value)
        {
            ISemanticNodeConverter.SourceTextBuilder.AddNodeInToStack(ISemanticNodeConverter.ConvertPABCNETNodeLocalBlockVariable("var_locblock", value));
        }
        public void visit(ICompiledConstructorCallAsConstant value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }
        public void visit(IRethrowStatement value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IForeachBreakNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public void visit(IForeachContinueNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }
		public void visit(INamespaceConstantReference value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
    	
		public void visit(IFunctionConstantReference value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
        
		public void visit(ICommonConstructorCallAsConstant value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
		
		public void visit(IArrayInitializer value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
		
		public void visit(ICommonEventNode value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
		
		public void visit(IEventNode value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
    	
		public void visit(ICompiledEventNode value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
    	
		public void visit(IStaticEventReference value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
    	
		public void visit(INonStaticEventReference value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}
		
		public void visit(IRecordInitializer value)
		{
            throw new System.NotSupportedException(value.GetType().ToString());
		}

        public virtual void visit(IDefaultOperatorNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }
        
        public virtual void visit(IAttributeNode value)
        {
        	throw new System.NotSupportedException(value.GetType().ToString());
        }
        
        public virtual void visit(IPInvokeStatementNode value)
        {
        	throw new System.NotSupportedException(value.GetType().ToString());
        }
        
        public virtual void visit(IBasicFunctionCallNodeAsConstant value)
        {
        	throw new System.NotSupportedException(value.GetType().ToString());
        }

        public virtual void visit(ICompiledStaticFieldReferenceNodeAsConstant value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public virtual void visit(ILambdaFunctionNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }

        public virtual void visit(ILambdaFunctionCallNode value)
        {
            throw new System.NotSupportedException(value.GetType().ToString());
        }
        public virtual void visit(ICommonNamespaceEventNode value)
        {
        }
    }
}
