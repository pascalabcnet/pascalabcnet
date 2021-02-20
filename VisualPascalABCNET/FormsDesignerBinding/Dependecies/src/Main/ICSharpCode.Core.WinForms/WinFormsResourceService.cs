﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ICSharpCode.Core.WinForms
{
	/// <summary>
	/// This Class contains two ResourceManagers, which handle string and image resources
	/// for the application. It do handle localization strings on this level.
	/// </summary>
	public static class WinFormsResourceService
	{
		static Dictionary<string, Icon> iconCache = new Dictionary<string, Icon>();
		static Dictionary<string, Bitmap> bitmapCache = new Dictionary<string, Bitmap>();
		
		static WinFormsResourceService()
		{
			ResourceService.ClearCaches += ResourceService_ClearCaches;
		}
		
		static void ResourceService_ClearCaches(object sender, EventArgs e)
		{
			lock (iconCache) {
				iconCache.Clear();
			}
			lock (bitmapCache) {
				bitmapCache.Clear();
			}
		}
		
		#region Font loading
		static Font defaultMonospacedFont;
		
		public static Font DefaultMonospacedFont {
			get {
				if (defaultMonospacedFont == null) {
					defaultMonospacedFont = LoadDefaultMonospacedFont(FontStyle.Regular);
				}
				return defaultMonospacedFont;
			}
		}
		
		/// <summary>
		/// Loads the default monospaced font (Consolas or Courier New).
		/// </summary>
		public static Font LoadDefaultMonospacedFont(FontStyle style)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT
			    && Environment.OSVersion.Version.Major >= 6)
			{
				return LoadFont("Consolas", 10, style);
			} else {
				return LoadFont("Courier New", 10, style);
			}
		}
		
		/// <summary>
		/// The LoadFont routines provide a safe way to load fonts.
		/// </summary>
		/// <param name="fontName">The name of the font to load.</param>
		/// <param name="size">The size of the font to load.</param>
		/// <returns>
		/// The font to load or the menu font, if the requested font couldn't be loaded.
		/// </returns>
		public static Font LoadFont(string fontName, int size)
		{
			return LoadFont(fontName, size, FontStyle.Regular);
		}
		
		/// <summary>
		/// The LoadFont routines provide a safe way to load fonts.
		/// </summary>
		/// <param name="fontName">The name of the font to load.</param>
		/// <param name="size">The size of the font to load.</param>
		/// <param name="style">The <see cref="System.Drawing.FontStyle"/> of the font</param>
		/// <returns>
		/// The font to load or the menu font, if the requested font couldn't be loaded.
		/// </returns>
		public static Font LoadFont(string fontName, int size, FontStyle style)
		{
			try {
				return new Font(fontName, size, style);
			} catch (Exception ex) {
				LoggingService.Warn(ex);
				return SystemInformation.MenuFont;
			}
		}
		
		/// <summary>
		/// The LoadFont routines provide a safe way to load fonts.
		/// </summary>
		/// <param name="fontName">The name of the font to load.</param>
		/// <param name="size">The size of the font to load.</param>
		/// <param name="unit">The <see cref="System.Drawing.GraphicsUnit"/> of the font</param>
		/// <returns>
		/// The font to load or the menu font, if the requested font couldn't be loaded.
		/// </returns>
		public static Font LoadFont(string fontName, int size, GraphicsUnit unit)
		{
			return LoadFont(fontName, size, FontStyle.Regular, unit);
		}
		
		/// <summary>
		/// The LoadFont routines provide a safe way to load fonts.
		/// </summary>
		/// <param name="fontName">The name of the font to load.</param>
		/// <param name="size">The size of the font to load.</param>
		/// <param name="style">The <see cref="System.Drawing.FontStyle"/> of the font</param>
		/// <param name="unit">The <see cref="System.Drawing.GraphicsUnit"/> of the font</param>
		/// <returns>
		/// The font to load or the menu font, if the requested font couldn't be loaded.
		/// </returns>
		public static Font LoadFont(string fontName, int size, FontStyle style, GraphicsUnit unit)
		{
			try {
				return new Font(fontName, size, style, unit);
			} catch (Exception ex) {
				LoggingService.Warn(ex);
				return SystemInformation.MenuFont;
			}
		}
		
		/// <summary>
		/// The LoadFont routines provide a safe way to load fonts.
		/// </summary>
		/// <param name="baseFont">The existing font from which to create the new font.</param>
		/// <param name="newStyle">The new style of the font.</param>
		/// <returns>
		/// The font to load or the baseFont (if the requested font couldn't be loaded).
		/// </returns>
		public static Font LoadFont(Font baseFont, FontStyle newStyle)
		{
			try {
				return new Font(baseFont, newStyle);
			} catch (Exception ex) {
				LoggingService.Warn(ex);
				return baseFont;
			}
		}
		#endregion
		
		/// <summary>
		/// Returns a icon from the resource database, it handles localization
		/// transparent for the user. In the resource database can be a bitmap
		/// instead of an icon in the dabase. It is converted automatically.
		/// </summary>
		/// <returns>
		/// The icon in the (localized) resource database, or null, if the icon cannot
		/// be found.
		/// </returns>
		/// <param name="name">
		/// The name of the requested icon.
		/// </param>
		public static Icon GetIcon(string name)
		{
			lock (iconCache) {
				Icon ico;
				if (iconCache.TryGetValue(name, out ico))
					return ico;
				
				object iconobj = ResourceService.GetImageResource(name);
				if (iconobj == null) {
					return null;
				}
				if (iconobj is Icon) {
					ico = (Icon)iconobj;
				} else {
					ico = BitmapToIcon((Bitmap)iconobj);
				}
				iconCache[name] = ico;
				return ico;
			}
		}
		
		/// <summary>
		/// Converts a bitmap into an icon.
		/// </summary>
		public static Icon BitmapToIcon(Bitmap bmp)
		{
			IntPtr hIcon = bmp.GetHicon();
			try {
				using (Icon tempIco = Icon.FromHandle(hIcon)) {
					// Icon.FromHandle creates a Icon object that uses the HIcon but does
					// not own it. We could leak HIcons on language changes.
					// We have no idea when we may dispose the icons after a language change
					// (they could still be used), so we'll have to create an owned icon.
					// Unfortunately, there's no Icon.FromHandle(IntPtr,bool takeOwnership) method.
					// We could use reflection to set the ownHandle field; or we create a copy of the icon
					// and immediately destroy the original
					return new Icon(tempIco, tempIco.Width, tempIco.Height);
				} // dispose tempico, doesn't do much because the icon isn't owned
			} finally {
				NativeMethods.DestroyIcon(hIcon);
			}
		}
		
		/// <summary>
		/// Returns a bitmap from the resource database, it handles localization
		/// transparent for the user.
		/// The bitmaps are reused, you must not dispose the Bitmap!
		/// </summary>
		/// <returns>
		/// The bitmap in the (localized) resource database.
		/// </returns>
		/// <param name="name">
		/// The name of the requested bitmap.
		/// </param>
		/// <exception cref="ResourceNotFoundException">
		/// Is thrown when the GlobalResource manager can't find a requested resource.
		/// </exception>
		public static Bitmap GetBitmap(string name)
		{
			lock (bitmapCache) {
				Bitmap bmp;
				if (bitmapCache.TryGetValue(name, out bmp))
					return bmp;
				//roman//
				/*bmp = (Bitmap)ResourceService.GetImageResource(name);
				if (bmp == null) {
					throw new ResourceNotFoundException(name);
				}*/
				bitmapCache[name] = bmp;
				return bmp;
			}
		}
        //roman//
        public static void AddToBitmapCache(string name, Bitmap bmp)
        {
            bitmapCache[name] = bmp;
        }
	}
}
