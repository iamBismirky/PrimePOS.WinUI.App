using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PrimePOS.Contracts.DTOs.Rol;
using PrimePOS.WinUI.Services.Api;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public partial class RolViewModel : ObservableObject
{
    private readonly RolApiService _service;
    //public LoadingService Loading { get; }
    public RolViewModel(RolApiService service)
    {
        _service = service;
        //Loading = loading;
        _ = CargarAsync();

    }
    public ObservableCollection<RolDto> Roles { get; } = new();

    [ObservableProperty]
    private RolDto? rolSeleccionado = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "";

    [ObservableProperty]
    private bool isOverlayOpen;
    [ObservableProperty]
    private bool isEditMode;
    [ObservableProperty]
    private RolDto rolForm = new RolDto();




    [RelayCommand]
    public async Task CargarAsync()
    {

        if (IsLoading) return;
        try
        {

            IsLoading = true;
            LoadingMessage = "Cargando roles...";


            var lista = await _service.GetRolesAsync();

            Roles.Clear();
            foreach (var rol in lista)
                Roles.Add(rol);
            IsLoading = false;

        }
        catch (Exception)
        {
            LoadingMessage = "Error al cargar los roles";
        }
        finally
        {
            IsLoading = false;

        }
    }

    [RelayCommand]
    public async Task DesactivarAsync()
    {
        //if (RolSeleccionado == null) return;

        //await _service.DesactivarRolAsync(RolSeleccionado.RolId);
        //await CargarAsync();
    }

    [RelayCommand]
    public void AbrirCrear()
    {
        RolForm = new RolDto();
        IsEditMode = false;
        IsOverlayOpen = true;
    }
    [RelayCommand]
    public void AbrirEditar(RolDto rol)
    {
        RolForm = new RolDto
        {
            RolId = rol.RolId,
            Nombre = rol.Nombre
        };

        IsEditMode = true;
        IsOverlayOpen = true;
    }
    [RelayCommand]
    public async Task GuardarAsync()
    {
        try
        {


            if (IsEditMode)
            {
                //await _service.ActualizarRolAsync(RolForm);
            }
            else
            {
                //await _service.CrearRolAsync(RolForm);
            }

            await CargarAsync();
            CerrarOverlay();
        }
        finally
        {

        }
    }
    [RelayCommand]
    public void CerrarOverlay()
    {
        IsOverlayOpen = false;
        RolForm = new RolDto();
    }
}