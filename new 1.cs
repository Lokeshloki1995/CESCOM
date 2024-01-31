<div style="margin-right:23%!important"class="text-center" align="center">
                                        <div class="span3">
                                        </div>
                                        
                                            <asp:Button ID="cmdSave" runat="server" Text="Recieve" CssClass="btn btn-success"
                                                OnClick="cmdSave_Click" OnClientClick ="Javascript:return ValidateForm()"/>
                                      
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                OnClick="cmdReset_Click" />
                                       
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>