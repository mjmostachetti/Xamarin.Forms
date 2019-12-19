using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	internal class CarouselViewCoreGalleryPage : CoreGalleryPage<CarouselView>
	{
		private object _currentItem;
		public ObservableCollection<CarouselData> ItemSourceUnderTest { get; set; }
		public CarouselView CarouselViewUnderTest { get; set; }

		protected override void InitializeElement(CarouselView element)
		{
			base.InitializeElement(element);

			element.IsScrollAnimated = true;
			element.ItemsSource = GetCarouselItems();
			element.ItemTemplate = GetCarouselTemplate();
			element.ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal);
			element.HeightRequest = 250;
		}

		protected override void Build(StackLayout stackLayout)
		{
			base.Build(stackLayout);

			ItemSourceUnderTest = new ObservableCollection<CarouselData>(GetCarouselItems());
			CarouselViewUnderTest = new CarouselView { HeightRequest = 250, IsScrollAnimated = false, IsSwipeEnabled = true, ItemsSource = ItemSourceUnderTest, ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal), ItemTemplate = GetCarouselTemplate(), CurrentItem = _currentItem };
			CarouselViewUnderTest.CurrentItemChanged += CarouselViewUnderTest_CurrentItemChanged;

			var currentItemContainer = new ValueViewContainer<CarouselView>(Test.CarouselView.CurrentItem, CarouselViewUnderTest, "CurrentItem", value => value.ToString());
			var isSwipeEnabledContainer = new ValueViewContainer<CarouselView>(Test.CarouselView.IsSwipeEnabled, new CarouselView { IsSwipeEnabled = false, HeightRequest = 250, ItemsSource = GetCarouselItems(), ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal), ItemTemplate = GetCarouselTemplate()}, "IsSwipeEnabled", value => value.ToString());
			var isScrollAnimatedContainer = new ValueViewContainer<CarouselView>(Test.CarouselView.IsScrollAnimated, new CarouselView { IsScrollAnimated = false, HeightRequest = 250, ItemsSource = GetCarouselItems(), ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal), ItemTemplate = GetCarouselTemplate() }, "IsScrollAnimated", value => value.ToString());
			var peekAreaInsetsContainer = new ValueViewContainer<CarouselView>(Test.CarouselView.PeekAreaInsets, new CarouselView { PeekAreaInsets = new Thickness(24, 12, 36, 6), HeightRequest = 250, ItemsSource = GetCarouselItems(), ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal), ItemTemplate = GetCarouselTemplate() }, "PeekAreaInsets", value => value.ToString());
			var positionContainer = new ValueViewContainer<CarouselView>(Test.CarouselView.Position, new CarouselView { Position = 2, HeightRequest = 250, ItemsSource = GetCarouselItems(), ItemsLayout = GetCarouselLayout(ItemsLayoutOrientation.Horizontal), ItemTemplate = GetCarouselTemplate() }, "Position", value => value.ToString());

			Add(currentItemContainer);
			Add(isSwipeEnabledContainer);
			Add(isScrollAnimatedContainer);
			Add(peekAreaInsetsContainer);
			Add(positionContainer);
		}

		public int Counter { get; set; } = 0;

		private void CarouselViewUnderTest_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
		{
			Counter++;
			ItemSourceUnderTest.Insert(0, new CarouselData { Name = $"Counter {Counter}", Color = Color.Red });
		}

		internal List<CarouselData> GetCarouselItems()
		{
			var random = new Random();

			var items = new List<CarouselData>();

			for (int n = 0; n < 5; n++)
			{
				items.Add(new CarouselData
				{
					Color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
					Name = DateTime.Now.AddDays(n).ToString("D")
				});
			}

			_currentItem = items[4];

			return items;
		}

		internal LinearItemsLayout GetCarouselLayout(ItemsLayoutOrientation orientation)
		{
			return new LinearItemsLayout(orientation)
			{
				SnapPointsType = SnapPointsType.MandatorySingle,
				SnapPointsAlignment = SnapPointsAlignment.Center
			};
		}

		internal DataTemplate GetCarouselTemplate()
		{
			return new DataTemplate(() =>
			{
				var grid = new Grid();

				var info = new Label
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Margin = new Thickness(6)
				};

				info.SetBinding(Label.TextProperty, new Binding("Name"));

				grid.Children.Add(info);

				var frame = new Frame
				{
					CornerRadius = 12,
					Content = grid,
					HasShadow = false,
					Margin = new Thickness(12)
				};

				frame.SetBinding(BackgroundColorProperty, new Binding("Color"));

				return frame;
			});
		}
	}

	[Preserve(AllMembers = true)]
	public class CarouselData
	{
		public Color Color { get; set; }
		public string Name { get; set; }
	}
}