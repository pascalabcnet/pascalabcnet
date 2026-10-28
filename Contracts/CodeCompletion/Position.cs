namespace PascalABCCompiler.Contracts
{
    public struct Position
    {
        public int line;
        public int column;
        public int end_line;
        public int end_column;
        public string file_name;
        public bool from_metadata;
        public string metadata;
        public string metadata_title;
        public string full_metadata_title;
        public string fold_text;
        public MetadataType metadata_type;

        public Position(int line, int column, int end_line, int end_column, string file_name)
        {
            this.line = line;
            this.column = column;
            this.end_line = end_line;
            this.end_column = end_column;
            this.file_name = file_name;
            this.from_metadata = false;
            this.metadata = null;
            this.metadata_title = null;
            this.fold_text = null;
            this.metadata_type = MetadataType.Unknown;
            this.full_metadata_title = null;
        }

        public Position(int line, int column, int end_line, int end_column, string file_name, string fold_text)
        {
            this.line = line;
            this.column = column;
            this.end_line = end_line;
            this.end_column = end_column;
            this.file_name = file_name;
            this.from_metadata = false;
            this.metadata = null;
            this.metadata_title = null;
            this.fold_text = fold_text;
            this.metadata_type = MetadataType.Unknown;
            this.full_metadata_title = null;
        }
    }
}
