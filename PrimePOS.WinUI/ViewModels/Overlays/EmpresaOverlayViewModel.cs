using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using PrimePOS.Contracts.DTOs.Empresa;
using PrimePOS.WinUI.Contracts;
using PrimePOS.WinUI.Services;
using PrimePOS.WinUI.Services.Api;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace PrimePOS.WinUI.ViewModels.Overlays
{
    public partial class EmpresaOverlayViewModel : ObservableObject, IOverlayViewModel
    {
        private readonly EmpresaApiService _empresaApi;
        private readonly OverlayService _overlaysService;
        private readonly NotificationService _notify;

        private readonly TaskCompletionSource<bool> _tcs = new();
        public Task<bool> WaitTask => _tcs.Task;
        public EmpresaOverlayViewModel(EmpresaApiService empresaApi, OverlayService overlaysService, NotificationService notify)
        {
            _empresaApi = empresaApi;
            _overlaysService = overlaysService;
            _notify = notify;
        }

        [ObservableProperty]
        private EmpresaDto? empresa;
        [ObservableProperty]
        private string nombre = "";
        [ObservableProperty]
        private string rnc = "";
        [ObservableProperty]
        private string telefono = "";
        [ObservableProperty]
        private string email = "";
        [ObservableProperty]
        private string direccion = "";
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TieneLogo))]
        private StorageFile? logoFile;
        [ObservableProperty]
        private string? logoPath;
        [ObservableProperty]
        private BitmapImage? logoPreview;
        [ObservableProperty]
        private bool activa = true;
        [ObservableProperty]
        private bool isLoading = false;
        public bool TieneLogo => LogoFile is not null;

        [RelayCommand]
        private async Task GuardarAsync()
        {
            try
            {
                IsLoading = true;

                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    _notify.Error("El nombre es requerido");
                    return;
                }

                if (Empresa == null)
                {
                    var result = await _empresaApi.CrearEmpresaAsync(
                        new CrearEmpresaDto
                        {
                            Nombre = Nombre,
                            RNC = Rnc,
                            Telefono = Telefono,
                            Direccion = Direccion,
                            Email = Email,
                            Activa = Activa
                        });

                    if (!result.Success)
                    {
                        _notify.Error(
                            result.Message ?? "Error al crear empresa");

                        return;
                    }

                    int empresaId = result.Data;

                    if (empresaId <= 0)
                    {
                        _notify.Error(
                            "No se pudo obtener el Id de la empresa.");

                        return;
                    }

                    if (LogoFile is not null)
                    {
                        using var stream =
                            await LogoFile.OpenStreamForReadAsync();

                        var logoResult =
                            await _empresaApi.SubirLogoAsync(
                                empresaId,
                                stream,
                                LogoFile.Name);

                        if (!logoResult.Success)
                        {
                            _notify.Error(
                                logoResult.Message ??
                                "Error al subir logo");

                            return;
                        }
                    }

                    _notify.Success(
                        result.Message ??
                        "Empresa creada exitosamente");
                }
                else
                {
                    var dto = new ActualizarEmpresaDto
                    {
                        EmpresaId = Empresa.EmpresaId,
                        Nombre = Nombre,
                        RNC = Rnc,
                        Telefono = Telefono,
                        Email = Email,
                        Direccion = Direccion,
                        Activa = Activa
                    };

                    var result =
                        await _empresaApi.ActualizarEmpresaAsync(
                            Empresa.EmpresaId,
                            dto);

                    if (!result.Success)
                    {
                        _notify.Error(
                            result.Message ??
                            "Error al actualizar empresa");

                        return;
                    }

                    if (LogoFile is not null)
                    {
                        using var stream =
                            await LogoFile.OpenStreamForReadAsync();

                        var logoResult =
                            await _empresaApi.SubirLogoAsync(
                                Empresa.EmpresaId,
                                stream,
                                LogoFile.Name);

                        if (!logoResult.Success)
                        {
                            _notify.Error(
                                logoResult.Message ??
                                "Error al subir logo");

                            return;
                        }
                    }

                    _notify.Success(
                        result.Message ??
                        "Empresa actualizada exitosamente");
                }

                Limpiar();

                Close(true);
            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private async Task SeleccionarLogoAsync()
        {
            try
            {
                var picker = new FileOpenPicker();

                var hwnd = WindowNative.GetWindowHandle(
                    App._window ?? throw new InvalidOperationException("Ventana principal no disponible"));

                InitializeWithWindow.Initialize(picker, hwnd);

                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");

                var file = await picker.PickSingleFileAsync();

                if (file is null)
                    return;

                LogoFile = file;
                LogoPath = file.Path;

                using var stream = await file.OpenAsync(FileAccessMode.Read);

                var bitmap = new BitmapImage();

                await bitmap.SetSourceAsync(stream);

                LogoPreview = new BitmapImage(new Uri(file.Path));
            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
            }
        }

        [RelayCommand]
        private void Cancelar()
        {
            Limpiar();

            Close(false);
        }
        public void Close(bool result = false)
        {
            _tcs.TrySetResult(result);
        }
        private void Limpiar()
        {
            Empresa = null;
            Nombre = "";
            Rnc = "";
            Telefono = "";
            Email = "";
            Direccion = "";
            LogoFile = null;
            LogoPath = null;
            Activa = true;
        }
    }
}
