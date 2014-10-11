using System;
using System.Windows.Shell;

namespace pw
{
    public static class JumpTaskExtension
    {
        /// <summary>
        /// Initializes an instance of the JumpTask class.
        /// </summary>
        /// <param name="jumpTask">The JumpTask to fill the initialize.</param>
        /// <param name="_sTitle">The text displayed for the task in the Jump List.</param>
        /// <param name="_sApplicationPath">The path to the application.</param>
        /// <param name="_sArguments">The arguments passed to the application on startup.</param>
        /// <param name="_sIconResourcePath">The path to a resource that contains the icon to display in the Jump List.</param>
        /// <param name="_iIconResourceIndex">The zero-based index of an icon embedded in a resource.</param>
        /// <returns>Returns the initialized JumpTask instance.</returns>
        public static JumpTask Initialized(this JumpTask jumpTask, string _sTitle, string _sApplicationPath, string _sArguments, string _sIconResourcePath, int _iIconResourceIndex)
        {
            jumpTask.Title = _sTitle;
            jumpTask.ApplicationPath = _sApplicationPath;
            jumpTask.Arguments = _sArguments;
            jumpTask.IconResourcePath = _sIconResourcePath;
            jumpTask.IconResourceIndex = _iIconResourceIndex;

            // Support chaining
            return jumpTask;
        }
    }
}
