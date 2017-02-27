using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace VoresCarlsberg.Application.Services
{
	public class Mailer
	{
		// -------------------------------------------------------------------------
		// Enums
		// -------------------------------------------------------------------------

		public enum MimeType
		{
			HTML,
			Plain
		};

		// -------------------------------------------------------------------------
		// Fields
		// -------------------------------------------------------------------------

		private string _mailserver;

		private string _finalBodyTemplate = null;
		private readonly string _bodyTemplate = null;
		private readonly Uri _bodyTemplateUri = null;
		private Encoding _bodyEncoding = Encoding.UTF8;
		private MimeType _bodyMimeType = MimeType.HTML;

		private string _finalAltBodyTemplate = null;
		private readonly string _altBodyTemplate = null;
		private readonly Uri _altBodyTemplateUri = null;
		private Encoding _altBodyEncoding = Encoding.UTF8;
		private MimeType _altMimeType = MimeType.Plain;

		private Attachment _attachment = null;
		private bool _useAsyncSend = true;

		// -------------------------------------------------------------------------
		// Constructor
		// -------------------------------------------------------------------------

		// -------------------------------------------------------------------------

		/// <summary>
		/// Instantiates a new Mailer object
		/// </summary>
		/// <param name="bodyTemplate">Mail Template</param>
		public Mailer(string bodyTemplate)
		{
			_bodyTemplate = bodyTemplate;
		}


		// -------------------------------------------------------------------------

		/// <summary>
		/// Instantiates a new Mailer object with alternate body
		/// </summary>
		/// <param name="mailserver">Smtp servername</param>
		/// <param name="bodyTemplate">Mail Template</param>
		/// <param name="altBodyTemplate">Alternate mail Template</param>
		public Mailer(string mailserver, string bodyTemplate, string altBodyTemplate)
			: this(bodyTemplate)
		{
			_altBodyTemplate = altBodyTemplate;
		}

		// -------------------------------------------------------------------------

		/// <summary>
		/// Instantiates a new Mailer object
		/// </summary>
		/// <param name="mailserver">Smtp servername</param>
		/// <param name="bodyTemplateUri">Uri where mail Template can be requested (only HTTP and File Uri's are allowed)</param>
		public Mailer(Uri bodyTemplateUri)
		{
			_bodyTemplateUri = bodyTemplateUri;
		}

		// -------------------------------------------------------------------------

		/// <summary>
		/// Instantiates a new Mailer object
		/// </summary>
		/// <param name="mailserver">Smtp servername</param>
		/// <param name="bodyTemplateUri">Uri where mail Template can be requested (only HTTP and File Uri's are allowed)</param>
		/// <param name="altBodyTemplateUri">Uri where alternate mail Template can be requested (only HTTP and File Uri's are allowed)</param>
		public Mailer(Uri bodyTemplateUri, Uri altBodyTemplateUri)
			: this(bodyTemplateUri)
		{
			_altBodyTemplateUri = altBodyTemplateUri;
		}

		// -------------------------------------------------------------------------
		// Events
		// -------------------------------------------------------------------------

		public event SendCompletedEventHandler SendCompleted = null;

		// -------------------------------------------------------------------------
		// Public members
		// -------------------------------------------------------------------------

		public bool UseAsyncSend
		{
			get { return _useAsyncSend; }
			set { _useAsyncSend = value; }
		}

		// -------------------------------------------------------------------------

		public void SendMessage(MailAddress fromEmail, MailAddress toEmail, string subject)
		{
			SendMessage(fromEmail, toEmail, subject, null);
		}

		// -------------------------------------------------------------------------

		public void SendMessage(MailAddress fromEmail, MailAddress toEmail, string subject, IDictionary<string, string> templateValues)
		{
			SendMessage(fromEmail, toEmail, subject, templateValues, toEmail);
		}

		// -------------------------------------------------------------------------

		public void SendMessage(MailAddress fromEmail, MailAddress toEmail, string subject, IDictionary<string, string> templateValues, object userToken)
		{
			MailMessage message = new MailMessage();
			message.Subject = subject;
			message.From = fromEmail;
			message.To.Add(toEmail);

			string bodyTemplate = BodyTemplate;
			if (templateValues != null)
				bodyTemplate = PrepareBodyTemplate(bodyTemplate, templateValues);

			if (HasAlternateBodyTemplate())
			{
				AlternateView bodyView = GetView(bodyTemplate, BodyEncoding, BodyMimeType);
				message.AlternateViews.Add(bodyView);

				string alternateBodyTemplate = AltBodyTemplate;
				if (templateValues != null)
					alternateBodyTemplate = PrepareBodyTemplate(alternateBodyTemplate, templateValues);

				AlternateView alternate = GetView(alternateBodyTemplate, AltBodyEncoding, AltMimeType);
				message.AlternateViews.Add(alternate);
			}
			else
			{
				message.Body = bodyTemplate;
				message.IsBodyHtml = BodyMimeType.Equals(MimeType.HTML);
				message.BodyEncoding = BodyEncoding;
			}

			// Attachment
			if (Attachment != null)
			{
				message.Attachments.Add(Attachment);
			}

			SmtpClient smtpClient = Mailserver != null ? new SmtpClient(Mailserver) : new SmtpClient();

			if (UseAsyncSend)
			{
				// Setup eventhandling
				if (SendCompleted != null)
					smtpClient.SendCompleted += SendCompleted;

				smtpClient.SendAsync(message, userToken);
			}
			else
			{
				smtpClient.Send(message);
			}
		}

		// -------------------------------------------------------------------------

		public void SendMessages(MailAddress fromEmail, MailAddress[] toEmails, string subject, IDictionary<string, string>[] templateValues)
		{
			SendMessages(fromEmail, toEmails, subject, templateValues, toEmails);
		}

		// -------------------------------------------------------------------------

		public void SendMessages(MailAddress fromEmail, MailAddress[] toEmails, string subject)
		{
			for (int i = 0; i < toEmails.Length; i++)
			{
				SendMessage(fromEmail, toEmails[i], subject, null);
			}
		}

		// -------------------------------------------------------------------------

		public void SendMessages(MailAddress fromEmail, MailAddress[] toEmails, string subject, IDictionary<string, string>[] templateValues, object[] userToken)
		{
			if (!(toEmails.Length == templateValues.Length && templateValues.Length == userToken.Length))
			{
				throw new IllegalInputException("toMails, templateValues and userToken collections need to have the same size");
			}

			for (int i = 0; i < toEmails.Length; i++)
			{
				SendMessage(fromEmail, toEmails[i], subject, templateValues[i], userToken[i]);
			}
		}

		// -------------------------------------------------------------------------

		public string Mailserver
		{
			get { return _mailserver; }
			set { _mailserver = value; }
		}

		// -------------------------------------------------------------------------

		public Encoding BodyEncoding
		{
			get { return _bodyEncoding; }
			set { _bodyEncoding = value; }
		}

		// -------------------------------------------------------------------------

		public MimeType BodyMimeType
		{
			get { return _bodyMimeType; }
			set { _bodyMimeType = value; }
		}

		// -------------------------------------------------------------------------

		public Encoding AltBodyEncoding
		{
			get { return _altBodyEncoding; }
			set { _altBodyEncoding = value; }
		}

		// -------------------------------------------------------------------------

		public MimeType AltMimeType
		{
			get { return _altMimeType; }
			set { _altMimeType = value; }
		}

		// -------------------------------------------------------------------------

		public Attachment Attachment
		{
			get { return _attachment; }
			set { _attachment = value; }
		}

		// -------------------------------------------------------------------------
		// Private members
		// -------------------------------------------------------------------------

		private string BodyTemplate
		{
			get
			{
				if (String.IsNullOrEmpty(_finalBodyTemplate))
				{
					if (!String.IsNullOrEmpty(_bodyTemplate))
						_finalBodyTemplate = _bodyTemplate;
					else
						_finalBodyTemplate = FetchBodyTemplateFromUri(_bodyTemplateUri);
				}

				return _finalBodyTemplate;
			}
		}

		// -------------------------------------------------------------------------

		private string AltBodyTemplate
		{
			get
			{
				if (String.IsNullOrEmpty(_finalAltBodyTemplate))
				{
					if (!String.IsNullOrEmpty(_altBodyTemplate))
						_finalAltBodyTemplate = _altBodyTemplate;
					else
						_finalAltBodyTemplate = FetchBodyTemplateFromUri(_altBodyTemplateUri);
				}

				return _finalAltBodyTemplate;
			}
		}

		// -------------------------------------------------------------------------

		private static AlternateView GetView(string bodyTemplate, Encoding encoding, MimeType mimeType)
		{
			return AlternateView.CreateAlternateViewFromString(bodyTemplate, encoding,
															   (mimeType.Equals(MimeType.HTML)
																	? MediaTypeNames.Text.Html
																	: MediaTypeNames.Text.Plain));
		}

		// -------------------------------------------------------------------------

		public static string FetchBodyTemplateFromUri(Uri templateUri)
		{
			if (templateUri.Scheme == Uri.UriSchemeHttp || templateUri.Scheme == Uri.UriSchemeHttps)
			{
				WebRequest req = WebRequest.Create(templateUri);
				WebResponse result = req.GetResponse();
				Stream ReceiveStream = result.GetResponseStream();

				StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);

				return sr.ReadToEnd();
			}
			else if (templateUri.Scheme == Uri.UriSchemeFile)
			{
				FileStream fs = File.OpenRead(templateUri.LocalPath);
				StreamReader reader = new StreamReader(fs, Encoding.UTF8);

				string template = reader.ReadToEnd();

				reader.Close();
				fs.Close();

				return template;
			}
			else
			{
				throw new UriNotSupportedException(templateUri.Scheme + " is not supported. Only " +
												   Uri.UriSchemeHttp + " and " + Uri.UriSchemeFile + "is supported");
			}
		}

		// -------------------------------------------------------------------------

		public static string PrepareBodyTemplate(string bodyTemplate, IEnumerable<KeyValuePair<string, string>> values)
		{
			foreach (KeyValuePair<string, string> keyValuePair in values)
			{
				bodyTemplate = bodyTemplate.Replace(keyValuePair.Key, keyValuePair.Value);
			}

			return bodyTemplate;
		}

		// -------------------------------------------------------------------------

		private bool HasAlternateBodyTemplate()
		{
			return (!String.IsNullOrEmpty(_altBodyTemplate) || _altBodyTemplateUri != null);
		}
	}

	internal class IllegalInputException : ApplicationException
	{
		public IllegalInputException(string message) : base(message) { }
	}

	public class UriNotSupportedException : ApplicationException
	{
		public UriNotSupportedException(string message) : base(message) { }
	}
}