using log4net.Core;
using log4net.Layout;


namespace LogClient.MongoDBLog

{
    public class MongoDBAppenderParameter
    {
        /// <summary>
        /// The name of this parameter.
        /// </summary>
        private string m_parameterName;

        /// <summary>
        /// The <see cref="T:log4net.Layout.IRawLayout" /> to use to render the
        /// logging event into an object for this parameter.
        /// </summary>
        private IRawLayout m_layout;

        /// <summary>
        /// Gets or sets the name of this parameter.
        /// </summary>
        /// <value>
        /// The name of this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The name of this parameter. The parameter name
        /// must match up to a named parameter to the SQL stored procedure
        /// or prepared statement.
        /// </para>
        /// </remarks>
        public string ParameterName
        {
            get
            {
                return this.m_parameterName;
            }
            set
            {
                this.m_parameterName = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:log4net.Layout.IRawLayout" /> to use to 
        /// render the logging event into an object for this 
        /// parameter.
        /// </summary>
        /// <value>
        /// The <see cref="T:log4net.Layout.IRawLayout" /> used to render the
        /// logging event into an object for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="T:log4net.Layout.IRawLayout" /> that renders the value for this
        /// parameter.
        /// </para>
        /// <para>
        /// The <see cref="T:log4net.Layout.RawLayoutConverter" /> can be used to adapt
        /// any <see cref="T:log4net.Layout.ILayout" /> into a <see cref="T:log4net.Layout.IRawLayout" />
        /// for use in the property.
        /// </para>
        /// </remarks>
        public IRawLayout Layout
        {
            get
            {
                return this.m_layout;
            }
            set
            {
                this.m_layout = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:log4net.Appender.AdoNetAppenderParameter" /> class.
        /// </summary>
        /// <remarks>
        /// Default constructor for the AdoNetAppenderParameter class.
        /// </remarks>
        public MongoDBAppenderParameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        public virtual object FormatValue(LoggingEvent loggingEvent)
        {
            object obj = this.Layout.Format(loggingEvent);
            return obj;
        }
    }
}
